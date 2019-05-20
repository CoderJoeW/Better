using System;
using System.Collections.Generic;

[Serializable]
public class PayPalErrorJsonResponse {

	public string name;
	public string message;
	public string information_link;
	public string debug_id;

	public List<Detail> details;

	[Serializable]
	public class Detail {
		public string field;
		public string issue;
	}

}