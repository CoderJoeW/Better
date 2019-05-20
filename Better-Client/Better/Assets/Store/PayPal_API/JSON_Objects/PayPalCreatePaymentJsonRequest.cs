﻿using System;
using System.Collections.Generic;

[Serializable]
public class PayPalCreatePaymentJsonRequest {

	public string intent;
	public Payer payer;
	public List<Transaction> transactions;
	public RedirectUrls redirect_urls;

	[Serializable]
	public class Payer {
		public string payment_method;
	}

	[Serializable]
	public class Transaction {
		public Amount amount;
		public ItemList item_list;
		public string description;
		public string invoice_number;
	}

	[Serializable]
	public class Amount {
		public string total;
		public string currency;
	}

	[Serializable]
	public class Item {
		public string name;
		public string description;
		public string quantity;
		public string price;
		public string currency;
	}

	[Serializable]
	public class ItemList {
		public List<Item> items;
	}

	[Serializable]
	public class RedirectUrls {
		public string return_url;
		public string cancel_url;
	}

}
	