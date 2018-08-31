import { trigger, state, transition, style, animate, group, query } from '@angular/animations';

export const depositLogAnimation =[ trigger(
    'enterAnimation', [
        transition(':enter', [
            style({ transform: 'translateX(-100%)', opacity: 0 }),
            animate('1000ms ease-in', style({ transform: 'translateX(0)', opacity: 1 }))
        ])
    ]),
    trigger('expandCollapse', [
        transition('* => *', [
            style({ opacity: 0.2, transform: 'translateX(-100%)' }),
            animate('1000ms linear', style({ transform: 'translateX(0)', opacity: 1 }))
        ])
    ])]