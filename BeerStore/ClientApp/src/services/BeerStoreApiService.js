export default class BeerStoreApiService {

  //_apiBase = 'http://terminal.chance-ltd.ru/api/v1/';
  _apiBase = 'http://laravel6.local/api/v1/';

  param(obj){
    let str = [];
    for (let p in obj){
      if (obj[p]) { str.push(p + '=' + obj[p]) }
    }
    return str.join('&')
  }

  async getResource(url, filter = {}) {
    const strFilter = this.param(filter);

    let resUrl = `${this._apiBase}${url}`;
    if (strFilter !== ''){
      resUrl += '?' + strFilter;
    }

    //const res = await fetch(`${this._apiBase}${url}`);
    const res = await fetch(resUrl);

    if (!res.ok) {
      throw new Error(`Could not fetch ${url}, received ${res.status}`)
    }
    return await res.json();
  }

  async getDepartments(page = 1, pageLimit = 20) {
    const filter = {
      page: page
    };

    let res = await this.getResource('departments', filter);

    res = this._transformPagination(res);
    res.data = res.data.map(this._transformDepartment)

    return res;
  }

  async getDepartment(id){
    let res = await this.getResource(`departments/${id}`);

    res = this._transformDepartment(res);

    return res;
  }

  async getInventories(department_id, page = 1, pageLimit = 20) {
    const filter = {
      department_id: department_id,
      page: page
    };

    let res = await this.getResource('inventories', filter);

    res = this._transformPagination(res);
    res.data = res.data.map(this._transformInventories)

    return res;
  }

  async getInventory(id){
    let res = await this.getResource(`inventories/${id}`);

    res = this._transformInventory(res);

    return res;
  }

  _transformDepartment = (department) => {
    return {
      id: department.id,
      name: department.descr
    }
  }

  _transformInventories = (inventory) => {
    return {
      id: inventory.id,
      number: inventory.number,
      date: inventory.date,
      quantity_mark: inventory.quantity_mark
    }
  }

  _transformInventory = (inventory) => {
    return {
      id: inventory.id,
      number: inventory.number,
      date: inventory.date,
      quantity_mark: inventory.quantity_mark,
      itemList: inventory.inventory_lines.map((item)=> {
        return {
          id: item.id,
          lineId: item.line_id,
          product: item.product_descr,
          quantity: item.quantity
        }
      })
    }
  }

  _transformPagination = (pagination) => {
    return {
      currentPageNumber: pagination.current_page,
      totalItems: pagination.total,
      totalPages: pagination.last_page,
      itemsPerPage: pagination.per_page,
      data: pagination.data
    }
  }
}