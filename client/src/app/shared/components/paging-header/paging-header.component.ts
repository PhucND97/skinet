import { Component, OnInit, Input } from '@angular/core';

import { ShopParams } from '../../models/shopParams';

@Component({
  selector: 'app-paging-header',
  templateUrl: './paging-header.component.html',
  styleUrls: ['./paging-header.component.scss']
})
export class PagingHeaderComponent implements OnInit {
  @Input() totalCount: number;
  @Input() pageIndex: number;
  @Input() pageSize: number;

  constructor() { }

  ngOnInit(): void {
  }

}
