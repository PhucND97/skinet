import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

import { IPagination, Pagination } from '../shared/models/Pagination';
import { IProductBrand } from '../shared/models/productBrand';
import { IProductType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.apiUrl;
  products: IProduct[] = [];
  types: IProductType[] = [];
  brands: IProductBrand[] = [];
  shopParams = new ShopParams();
  pagination = new Pagination();

  constructor(private http: HttpClient) { }

  getProducts(useCache: boolean) {
    if (useCache === false) {
      this.products = [];
    }

    if (this.products.length > 0 && useCache === true) {
      const pageReceived = Math.ceil(this.products.length / this.shopParams.pageSize);
      if (this.shopParams.pageIndex <= pageReceived) {
        this.pagination.data =
          this.products.slice((this.shopParams.pageIndex - 1) * this.shopParams.pageSize,
            this.shopParams.pageIndex * this.shopParams.pageSize);
        return of(this.pagination);
      }
    }
    let params = new HttpParams();



    if (this.shopParams.brandId !== 0) {
      params = params.append('brandId', this.shopParams.brandId.toString());
    }

    if (this.shopParams.typeId !== 0) {
      params = params.append('typeId', this.shopParams.typeId.toString());
    }

    if (this.shopParams.search) {
      params = params.append('search', this.shopParams.search);
    }

    params = params.append('sort', this.shopParams.sort);
    params = params.append('pageIndex', this.shopParams.pageIndex.toString());
    params = params.append('pageSizse', this.shopParams.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'products', { observe: 'response', params })
      .pipe(
        map(response => {
          this.products = [...this.products, ...response.body.data];
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }

  getShopParams() {
    return this.shopParams;
  }

  setShopParams(shopParams: ShopParams) {
    this.shopParams = shopParams;
  }

  getProduct(id: number) {
    const product = this.products.find(p => p.id === id);
    if (product) {
      return of(product);
    }
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  getBrands() {
    if (this.brands.length > 0) {
      return of(this.brands);
    }
    return this.http.get<IProductBrand[]>(this.baseUrl + 'products/brands').pipe(
      map(response => {
        this.brands = response;
        return response;
      })
    );
  }

  getTypes() {
    if (this.types.length > 0) {
      return of(this.brands);
    }
    return this.http.get<IProductType[]>(this.baseUrl + 'products/types').pipe(
      map(response => {
        this.types = response;
        return response;
      })
    );
  }
}
