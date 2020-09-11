import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Observable } from 'rxjs';

import { IBasket, IBasketItem } from '../../models/baskets';
import { BasketService } from 'src/app/basket/basket.service';
import { IOrderItem } from '../../models/order';


@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {
  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter();
  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter();
  @Input() isBasket = true;
  @Input() isOrder = false;
  @Input() items: IBasketItem[] | IOrderItem[] = [];


  constructor() { }

  ngOnInit(): void {
  }

  decrementItemQuantity(item: IBasketItem) {
    this.decrement.emit(item);
  }

  incrementItemQuantity(item: IBasketItem) {
    this.increment.emit(item);
  }

  removeItemFromBasket(item: IBasketItem) {
    this.remove.emit(item);
  }
}
