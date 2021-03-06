﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using BraintreeHttp;

namespace Samples.CaptureIntentExamples
{
    public class CaptureOrderSample
    {

        /*
            Method to capture order after creation. Valid approved order Id should be
	         passed an argument to this method.
        */
        public async static Task<HttpResponse> CaptureOrder(string OrderId, bool debug = false)
        {
            var request = new OrdersCaptureRequest(OrderId);
            request.Prefer("return=representation");
            request.RequestBody(new OrderActionRequest());
            var response = await PayPalClient.client().Execute(request);

            if (debug)
            {
                var result = response.Result<Order>();
                Console.WriteLine("Status: {0}", result.Status);
                Console.WriteLine("Order Id: {0}", result.Id);
                Console.WriteLine("Intent: {0}", result.Intent);
                Console.WriteLine("Links:");
                foreach (LinkDescription link in result.Links)
                {
                    Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
                }
                Console.WriteLine("Capture Ids: ");
                foreach (PurchaseUnit purchaseUnit in result.PurchaseUnits)
                {
                    foreach (Capture capture in purchaseUnit.Payments.Captures)
                    {
                        Console.WriteLine("\t {0}", capture.Id);
                    }
                }
                AmountWithBreakdown amount = result.PurchaseUnits[0].Amount;
                Console.WriteLine("Buyer:");
                Console.WriteLine("\tEmail Address: {0}\n\tName: {1}\n\tPhone Number: {2}{3}", result.Payer.EmailAddress, result.Payer.Name.FullName, result.Payer.Phone.CountryCode, result.Payer.Phone.NationalNumber);
                Console.WriteLine("Response JSON: \n {0}", PayPalClient.ObjectToJSONString(result));
            }

            return response;
        }

        /*
            Driver Function to invoke capture payment on order.
            Order Id should be replaced with the valid approved order id. 
        */
        // static void Main(string[] args)
        // {
        //     string OrderId = "5JC45524FD4582424";
        //     CaptureOrder(OrderId, true).Wait();
        // }
    }
}
