import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { CheckoutService } from '../checkout.service';
import { IDeliveryMethods } from 'src/app/shared/models/deliveryMethods';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.scss']
})
export class CheckoutDeliveryComponent implements OnInit {
  @Input() checkoutForm: FormGroup;
  deliveryMethods: IDeliveryMethods[];

  constructor(private checkoutService: CheckoutService, private basketService: BasketService) { }

  ngOnInit(): void {
    this.getDeliveryMethods();
  }

  getDeliveryMethods() {
    this.checkoutService.getDeliveryMethod().subscribe((dm: IDeliveryMethods[]) => {
      this.deliveryMethods = dm;
    }, error => {
      console.log(error);
    });
  }

  setDeliveryPrice(deliveryMethod: IDeliveryMethods) {
    this.basketService.setDeliveryPrice(deliveryMethod);
  }
}
