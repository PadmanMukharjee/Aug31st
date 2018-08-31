export class Helper {

    /*------ region public Methods --------*/

    /**
     *  String Format method
     * @param value
     * @param args
     */
    public static format(value, ...args): string {
        try {
            let val = '';
            let matches = value.match(/{(\d+)}/g);

            if (matches && matches.length > 0) {
                val = value;
                for (let i = 0; i < matches.length; i++) {
                    val = val.replace(matches[i], args[i]);
                }
            } else {
                return value;
            }
            return val;

        } catch (e) {
            return '';
        }
    }
    public static getQueryFormDate(date: Date) {
        try {
            return date.toISOString();
        } catch (e) {
            return '';
        }
    }
 /*------ end region public Methods --------*/

}
