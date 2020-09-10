import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket, IBasketItem } from '../../models/baskets';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {
  @Input() basket$: Observable<IBasket>;
  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter();
  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter();
  @Input() isBasket = true;

  constructor(private basketSevice: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketSevice.basket$;
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
