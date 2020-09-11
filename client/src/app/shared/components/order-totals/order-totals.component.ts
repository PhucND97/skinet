import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasketTotal } from '../../models/baskets';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-order-totals',
  templateUrl: './order-totals.component.html',
  styleUrls: ['./order-totals.component.scss']
})
export class OrderTotalsComponent implements OnInit {
  @Input() shippingPrice;
  @Input() subtotal;
  @Input() total;

  constructor() { }

  ngOnInit(): void {
  }

}
