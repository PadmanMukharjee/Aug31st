import { AfterContentInit, AfterViewInit, ElementRef, OnInit, style, ViewChild, ViewEncapsulation } from '@angular/core';
import { Http } from '@angular/http';
import {
    ChangeDetectorRef,
    Component,
    HostListener,
    Inject,
    Input
} from '@angular/core';
import * as PDFJS from 'pdfjs-dist';

@Component({
    selector: 'pdf-viewer-inline',
    encapsulation: ViewEncapsulation.None,
    styles: [`
    .textLayer {display: none}`],
    template: `
    <div>
    <div id="container" class="pdfViewer col-md-12" #pdfCanvas style="border:1px solid black;"></div></div>
  `
})
export class PDFViewerInlineComponent implements AfterViewInit {

    /*-----region Input/Output bindings -----*/
    @ViewChild('pdfCanvas') PdfCanvas: ElementRef;
    @Input('src') Source: string;
    /*-----endregion Input/Output bindings -----*/

    /*------ region constructor ------*/
    constructor(private _http: Http,
        @Inject(ChangeDetectorRef) private cdRef: ChangeDetectorRef) {
        console.log('pdfjs', PDFJS);
    }
    /*------ end region constructor ------*/

    /*------ region life cycle hooks ------*/
    ngAfterViewInit(): void {
        this.loadPDFDocument();
    }
    /*------ end region life cycle hooks ------*/

    /*------ region Public methods ------*/
    loadPDFDocument() {
        let pdfAsDataUri = 'data:application/pdf;base64,' + this.replaceAll(this.Source, '"', '');
        let pdfAsArray = this.convertDataURIToBinary(pdfAsDataUri);
        let currentPage = 1;
        let pages = [];

        PDFJS.getDocument({ data: pdfAsArray }).then(function (pdf) {
            pdf.getPage(currentPage).then(renderPage);
            let container = <HTMLSelectElement>document.getElementById('container');
            function renderPage(page) {               
                // var height = 700;
                // var viewport = page.getViewport(1);
                // var scale = height / viewport.height;
                let scale = 1.0;
                let scaledViewport = page.getViewport(scale);

                let canvas = document.createElement('canvas');
                let context = canvas.getContext('2d');
                canvas.height = scaledViewport.height;
                canvas.width = scaledViewport.width;

                let renderContext = {
                    canvasContext: context,
                    viewport: scaledViewport
                };
                page.render(renderContext).then(function () {
                    if (currentPage <= pdf.numPages) {
                        pages[currentPage] = canvas;
                        currentPage++;
                        if (currentPage <= pdf.numPages) {
                            pdf.getPage(currentPage).then(renderPage);
                        } else {
                            for (let i = 1; i < pages.length; i++) {
                                container.appendChild(pages[i]);
                            }
                        }
                    }
                });
            }
        });
    }

    replaceAll(str, find, replace) {
        return str.replace(new RegExp(find, 'g'), replace);
    }

    convertDataURIToBinary(dataURI) {
        let BASE64_MARKER = ';base64,';
        let base64Index = dataURI.indexOf(BASE64_MARKER) + BASE64_MARKER.length;
        let base64 = dataURI.substring(base64Index);
        let raw = atob(base64);
        let rawLength = raw.length;
        let array = new Uint8Array(new ArrayBuffer(rawLength));
        for (let i = 0; i < rawLength; i++) {
            array[i] = raw.charCodeAt(i);
        }
        return array;
    }
    /*------ end region Public methods ------*/

}
