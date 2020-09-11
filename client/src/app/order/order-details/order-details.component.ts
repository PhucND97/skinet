import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrderService } from '../order.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { IOrder } from 'src/app/shared/models/order';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  order: IOrder;

  constructor(
    private activatedRoute: ActivatedRoute,
    private orderService: OrderService,
    private bcService: BreadcrumbService,
    private basketService: BasketService) {
    this.bcService.set('@orderDetails', '');
  }

  ngOnInit(): void {
    this.loadOrder();
  }

  loadOrder() {
    this.orderService.getOrderById(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe((order: IOrder) => {
      this.order = order;
      this.bcService.set('@orderDetails', 'Order# ' + order.id + ' - ' + order.status);
      console.log(this.order);
    }, error => {
      console.log(error);
    });
  }
}
