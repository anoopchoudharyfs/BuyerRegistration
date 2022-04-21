Feature: Create or Update Bidders Approval Status
	As a Platform User
	I want to create or Update a bidder's Status (i.e Approved, Pending, Denied) record in a bidder status datastore
	So that that bidder's status can be verified ahead of or at the time of placing bids by checking against the bidder's record stored in the datastore

Background:
	Given my request body is
		"""
		{
			"MarketIdentityCode": 201,
			"TenderId" : 123,
			"TenderHouseId" : 20,
			"customerId" : "a_Customer_id",
			"BuyerId" :"a_Bidder_id",
			"Status": "Approved", 
			"BuyerRef": "10C",  
			"Action" : "test html"
		}
		"""	
	And my required headers
		| Key                   | Value |
		| x-bid-source-platform | 10    |
		| x-bid-client-id       | 1     |
		| x-bid-client-ip       | 1     |
		| x-bid-app-id          | 1     |
		| x-bid-user-reference  | 1     |

Scenario:Request with valid details
	When you send a put bidder request to /v1/bidder
	Then response should be 200 OK
	And the bidder should be persisted in the database as
	| PartitionKey      | Id                    | MarketIdentityCode | TenderId | TenderHouseId | customerId    | BuyerId     | Status   | BuyerRef | Action    |
	| a_Customer_id-201 | 123-a_Customer_id-201 | 201                | 123      | 20            | a_Customer_id | a_Bidder_id | Approved | 10C      | test html |


Scenario:Request with Invalid details
	 Given my request body is
		"""
		{			
			"MarketIdentityCode": 201,
			"TenderId" : "invalidauctionid",
			"TenderHouseId" : 20,
			"customerId" : "a_Customer_id",
			"BuyerId" :"a_Bidder_id",
			"Status": "Approved", 
			"BuyerRef": "10C",  
			"cta" : "test html"
		}
		"""	
	When you send a put bidder request to /v1/bidder
	Then response should be 400 Bad Request

	
Scenario:Request with Invalid Status
	 Given my request body is
		"""
		{
			"MarketIdentityCode": 201,
			"TenderId" : "30",
			"TenderHouseId" : 20,
			"customerId" : "a_Customer_id",
			"BuyerId" :"a_Bidder_id",
			"Status": "Invalid_Status", 
			"BuyerRef": "10C",  
			"Action" : "test html"
		}
		"""	
	When you send a put bidder request to /v1/bidder
	Then response should be 400 Bad Request
	And the response contains only these validation errors
		| code | value              | path     | description                                                                                                                                                                 |
		| 100  | ERROR_MISSING_DATA | $.Status | The JSON value could not be converted to System.Nullable`1[BuyerRegistration.Domain.Status]|

Scenario:Request when Status is None
	 Given my request body is
		"""
		{
			"MarketIdentityCode": 201,
			"TenderId" : "30",
			"TenderHouseId" : 20,
			"customerId" : "a_Customer_id",
			"BuyerId" :"a_Bidder_id",
			"Status": "None", 
			"BuyerRef": "10C",  
			"Action" : "test html"
		}
		"""	
	When you send a put bidder request to /v1/bidder
	Then response should be 400 Bad Request
	And the response contains only these validation errors with error codes
		| code | value                | path   | description                         |
		| 1008 | ERROR_INVALID_STATUS | Status | The Status field should not be None |

Scenario:Request with Pending Status requires a cta
	 Given my request body is
		"""
		{
			"MarketIdentityCode": 201,
			"TenderId" : "30",
			"TenderHouseId" : 20,
			"customerId" : "a_Customer_id",
			"BuyerId" :"a_Bidder_id",
			"Status": "Pending", 
			"BuyerRef": "10C",  
			"Action" : " "
		}
		"""	
	When you send a put bidder request to /v1/bidder
	Then response should be 400 Bad Request
	And the response contains only these validation errors
		| code | value              | path | description                                                          |
		| 100  | ERROR_MISSING_DATA | Action  | The Action field should not be empty when Status is Pending or Declined |

Scenario:Request with Denied Status requires a cta
	 Given my request body is
		"""
		{
			"MarketIdentityCode": 201,
			"TenderId" : 30,
			"TenderHouseId" : 20,
			"customerId" : "a_Customer_id",
			"BuyerId" :"a_Bidder_id",
			"Status": "Denied", 
			"BuyerRef": "10C",  
			"Action" : " "
		}
		"""	
	When you send a put bidder request to /v1/bidder
	Then response should be 400 Bad Request
	And the response contains only these validation errors
		| code | value              | path   | description                                                             |
		| 100  | ERROR_MISSING_DATA | Action | The Action field should not be empty when Status is Pending or Declined |



Scenario: Empty string request
	Given my request body is
		"""
		"""
	When you send a put bidder request to /v1/bidder
	Then response should be 400 Bad Request


Scenario: Empty json object request
	Given my request body is
		"""
		{}
		"""
	When you send a put bidder request to /v1/bidder
	Then response should be 400 Bad Request
	And the response contains only these validation errors
		| code | value              | path               | description                               |
		| 100  | ERROR_MISSING_DATA | MarketIdentityCode | The MarketIdentityCode field is required. |
		| 100  | ERROR_MISSING_DATA | TenderId           | The TenderId field is required.           |
		| 100  | ERROR_MISSING_DATA | TenderHouseId      | The TenderHouseId field is required.      |
		| 100  | ERROR_MISSING_DATA | CustomerId         | The CustomerId field is required.         |
		| 100  | ERROR_MISSING_DATA | BuyerId            | The BuyerId field is required.            |
		| 100  | ERROR_MISSING_DATA | Status             | The Status field is required.             |
		| 100  | ERROR_MISSING_DATA | BuyerRef           | The BuyerRef field is required.           |
		
			
Scenario: Request without required headers
	And the correlation id is "1211"
	And the current time is 2021-11-04T11:25:43.263Z
	And my required headers
		| Key | Value |
		
	When you send a put bidder request to /v1/bidder
	Then response should be 400 Bad Request


Scenario: Request with marketplace code that doesnt exist fails.
Given my request body is
		"""
		{
			"MarketIdentityCode": 9999,
			"TenderId" : 123,
			"TenderHouseId" : 20,
			"customerId" : "a_Customer_id",
			"BuyerId" :"a_Bidder_id",
			"Status": "Approved", 
			"BuyerRef": "10C",  
			"Action" : " "
		}
		"""	
	And my required headers
		| Key                   | Value |
		| x-bid-source-platform | 10    |
		| x-bid-client-id       | 1     |
		| x-bid-client-ip       | 1     |
		| x-bid-app-id          | 1     |
		| x-bid-user-reference  | 1     |

	When you send a put bidder request to /v1/bidder
	Then response should be 400 Bad Request
	And the response contains only these validation errors with error codes
		| code | value                              | path               | description                        |
		| 1010 | ERROR_NOTEXISTS_MARKETIDENTITYCODE | MarketIdentityCode | MarketIdentityCode does not exist. |


Scenario: Request with invalid marketplace code fails.
Given my request body is
		"""
		{
			"MarketIdentityCode": 0,
			"TenderId" : 123,
			"TenderHouseId" : 20,
			"customerId" : "a_Customer_id",
			"BuyerId" :"a_Bidder_id",
			"Status": "Approved", 
			"BuyerRef": "10C",  
			"Action" : "test html"
		}
		"""	
	And my required headers
		| Key                   | Value |
		| x-bid-source-platform | 10    |
		| x-bid-client-id       | 1     |
		| x-bid-client-ip       | 1     |
		| x-bid-app-id          | 1     |
		| x-bid-user-reference  | 1     |

	When you send a put bidder request to /v1/bidder
	Then response should be 400 Bad Request
	And the response contains only these validation errors with error codes
		| code | value                              | path               | description                                              |
		| 1006 | ERROR_INVALID_MARKETIDENTITYCODE   | MarketIdentityCode | The MarketIdentityCode field should be greater than zero |
		| 1010 | ERROR_NOTEXISTS_MARKETIDENTITYCODE | MarketIdentityCode | MarketIdentityCode does not exist.                       |



