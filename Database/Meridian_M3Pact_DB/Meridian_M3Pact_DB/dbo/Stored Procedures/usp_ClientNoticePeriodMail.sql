CREATE PROCEDURE usp_ClientNoticePeriodMail
AS 
BEGIN

DECLARE 
@clientId INT,
@ClientName VARCHAR(50),
@RelationshipManager VARCHAR(50),
@ContractEndDate DATE,
@MailiD VARCHAR(50),
@noticePeriod INT,
@returnCode INT

IF OBJECT_ID('tempdb..#Clients') IS NOT NULL DROP TABLE #Clients

SELECT * INTO #Clients FROM(
select Name,ClientCode, ContractEndDate,NoticePeriod,ContractEndDate as [noticedate] ,ClientID from Client
where IsActive='A'
and ContractEndDate>= CONVERT(date, getdate()))A

--- Updating the notice date based on mail count
update c
SET c.noticedate= 
CASE WHEN (c.noticePeriod+30)-(30*B.MailCount) >=0
THEN DATEADD(DAY, -(c.noticePeriod+30-(30*B.MailCount)),C.ContractEndDate)
ELSE
C.ContractEndDate
END
FROM #Clients c
JOIN (SELECT C.ClientId,COUNT(CN.ClientId) AS MailCount from #Clients C
LEFT JOIN ClientNoticePeriodMail CN
on C.ClientID=CN.ClientId
WHERE CN.ClientID IS NULL
AND C.ContractEndDate>= CONVERT(date, getdate())
Group by C.ClientId
UNION ALL
SELECT C.ClientId,COUNT(CN.ClientId) AS MailCount from #Clients C
LEFT JOIN ClientNoticePeriodMail CN
on C.ClientID=CN.ClientId
AND C.ContractEndDate>= CONVERT(date, getdate())
AND C.NoticePeriod=CN.NOticePeriod
AND C.ContractEndDate=CN.ContractEndDate
Group by C.ClientId) B
on c.ClientID =B.ClientID

UPDATE #clients
SET noticedate=  CONVERT(date, getdate())
WHERE ContractEndDate= CONVERT(date, getdate())

WHILE (SELECT Count(ClientCode) from #Clients WHERE noticedate < CONVERT(date, getdate())) >0
BEGIN
UPDATE #Clients
SET noticedate= DATEADD(day,30,noticedate)
WHERE noticedate < CONVERT(date, getdate())
END

IF OBJECT_ID('tempdb..#Alert') IS NOT NULL DROP TABLE #Alert

SELECT * INTO #Alert FROM 
(select * from #Clients
where noticeDate=CONVERT(date, getdate())
and noticedate<> ContractEndDate)A

IF OBJECT_ID('tempdb..#Expiry') IS NOT NULL DROP TABLE #Expiry

SELECT * INTO #Expiry FROM
(select * from #Clients
where noticeDate=CONVERT(date, getdate())
and noticedate= ContractEndDate) B

DECLARE @tableHTML  NVARCHAR(MAX) ;  

DECLARE Alert_Cursor CURSOR FOR  
SELECT clientId  
FROM #Alert
 
OPEN Alert_Cursor;  
FETCH NEXT FROM Alert_Cursor 
INTO @clientId
  
WHILE @@FETCH_STATUS = 0  
BEGIN  
SELECT @ClientName= Name,@ContractEndDate=ContractEndDate,@noticePeriod=C.NoticePeriod FROM CLIENT C
join UserLogin ul
on Ul.ID= C.RelationShipManagerID
where ClientID= @clientId  

set @MailiD=''  

select @MailiD=@MailiD+ u.Email +';' from ClientUserNoticeAlerts cn
join UserLogin u
on u.ID=cn.UserLoginId
where cn.ClientId=@clientId

-- Mail Body
SET @tableHTML =  
    N'Hi All,' +    
    N'<br/>' +  
    N'Please note that the contract of client '+@ClientName+ ' will expire on '+CAST( @ContractEndDate AS nvarchar) +'.<br/>' +     
    N'This is a reminder mail to renew the contract with'+@ClientName+ ' <br/>' +  
	N'If the contract has renewed, kindly update the contract end date field in M3Pact application, to avoid receiving further mails.<br/>'+
	N'Kindly ignore this e-mail if the contract will expire on '+ CAST( @ContractEndDate AS nvarchar) +'.<br/>' + 	
    N'</ul>' +
	N'<br/>' +
	N'<br/>' +
	N'Please do not reply to this e-mail, this is auto-generated. In case you have any queries / responses, please go to <a href="http://10.101.4.37">M3Pact</a> Application.'

	EXEC @returnCode= msdb.dbo.sp_send_dbmail
    @profile_name = 'Client notice period alert',
    @recipients=@MailiD,  
    @subject = 'Reminder: Renew Client Contract',  
    @body =@tableHTML,  
    @body_format = 'HTML' ;  

	IF @returnCode=0
	BEGIN
	INSERT INTO ClientNoticePeriodMail 
	VALUES(@clientId,@ContractEndDate,GETDATE(),'Notice Period Alert',@noticePeriod,GETDATE())
	END	

      FETCH NEXT FROM Alert_Cursor;  
   END;  
CLOSE Alert_Cursor;  
DEALLOCATE Alert_Cursor;  

set @MailiD=''
select @MailiD=@MailiD+ u.Email +';' from Roles R
join UserLogin u
on u.RoleID=r.RoleID
where r.RoleDesc='Admin'

DECLARE Expiry_Cursor CURSOR FOR  
SELECT clientId  
FROM #Expiry
 
OPEN Expiry_Cursor;  
FETCH NEXT FROM Expiry_Cursor 
INTO @clientId
  
WHILE @@FETCH_STATUS = 0  
   BEGIN  
SELECT @ClientName= Name,@ContractEndDate=ContractEndDate,@noticePeriod=C.NoticePeriod FROM CLIENT C
join UserLogin ul
on Ul.ID= C.RelationShipManagerID
where ClientID= @clientId     

-- Expired clients mail body
SET @tableHTML =  
    N'Hi All,' +   
    N'<br/>' +  
    N'This is to inform that the contract of client '+@ClientName+ ' has expired dated '+CAST( @ContractEndDate AS nvarchar)+'.<br/>' +     
    N'To avoid users to view this client in M3Pact application, kindly make the client Inactive in M3Pact Application. <br/>' +  
	N'To make the client Inactive,<br/>'+
	N'<ul>'+
	N'<li> Login to M3Pact  </li>'+
	N'<li> Go to Clients - View / Edit Clients  </li>'+
	N'<li> Turn the Active / Inactive switch on Client data page off </li>'+
	N'<li> Click Save  </li>'+
    N'</ul>' +
	N'<br/>' +
	N'<br/>' +
	N'Please do not reply to this e-mail, this is auto-generated. In case you have any queries / responses, please go to <a href="http://10.101.4.37">M3Pact</a> Application.'

	EXEC @returnCode= msdb.dbo.sp_send_dbmail
    @profile_name = 'Client notice period alert',
    @recipients=@MailiD,  
    @subject = 'Client Contract Expired',  
    @body =@tableHTML,  
    @body_format = 'HTML' ;  

	IF @returnCode=0
	BEGIN
	INSERT INTO ClientNoticePeriodMail 
	VALUES(@clientId,@ContractEndDate,GETDATE(),'Notice Period Alert',@noticePeriod,GETDATE())
	END	

      FETCH NEXT FROM Expiry_Cursor;  
   END;  
CLOSE Expiry_Cursor;  
DEALLOCATE Expiry_Cursor;  
END  