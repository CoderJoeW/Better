﻿using System;
using System.Collections.Generic;


[Serializable]
public class PayPalShowPaymentJsonResponse {

	public string id;
	public string intent;
	public string state;
	public string cart;
	public string create_time;
	public string update_time;
	public Payer payer;
	public List<Transaction> transactions;
	public RedirectURLs redirect_urls;
	public List<Link> links;


	[Serializable]
	public class Link {
		public string href;
		public string rel;
		public string method;
	}
		
	[Serializable]
	public class RedirectURLs {
		public string return_url;
		public string cancel_url;
	}

	[Serializable]
	public class Transaction {

		public Amount amount;
		public Payee payee;
		public string description;
		public string invoice_number;
		public ItemList item_list;

	}

	[Serializable]
	public class Item {
		public string name;
		public string description;
		public string price;
		public string currency;
		public string quantity;
	}

	[Serializable] 
	public class ShippingAddress {
		public string recipient_name;
		public string line1;
		public string city;
		public string state;
		public string postal_code;
		public string country_code;
	}

	[Serializable]
	public class ItemList {
		public List<Item> items;
		public ShippingAddress shipping_address;
	}

	[Serializable]
	public class Amount {
		public string total;
		public string currency;
	}

	[Serializable]
	public class Payee {
		public string email;
	}

	[Serializable]
	public class Payer {
		public string payment_method;
		public string status;
		public PayerInfo payer_info; 
	}


	[Serializable]
	public class PayerInfo {
		public string email;
		public string first_name;
		public string payer_id;
		public string country_code;
	}

}