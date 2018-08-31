import { Directive, ElementRef, Input } from '@angular/core';

@Directive({
    selector: '[readonly]'
})
export class ReadOnlyDirective {
    private _readonly;
    constructor(private el: ElementRef) {
    }

    ngOnInit() {
        if (this._readonly || typeof this._readonly === 'undefined') {
            this.el.nativeElement.setAttribute('readonly', 1);
        } else {
            this.el.nativeElement.removeAttribute('readonly');
        }

    }

    @Input() set readonly(condition: boolean) {
        this._readonly = condition != false;
    }
}
