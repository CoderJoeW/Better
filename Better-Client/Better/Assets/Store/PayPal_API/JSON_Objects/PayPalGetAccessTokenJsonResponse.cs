using System;
using System.Collections.Generic;

[Serializable]
public class PayPalGetAccessTokenJsonResponse{
	public string scope;
	public string access_token;
	public string token_type;
	public string app_id;
	public string expires_in;
}