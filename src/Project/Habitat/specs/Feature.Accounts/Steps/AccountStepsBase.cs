﻿using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Sitecore.Feature.Accounts.Specflow.Infrastructure;
using Sitecore.Feature.Demo.Specflow;
using Sitecore.Foundation.Common.Specflow.Infrastructure;
using Sitecore.Foundation.Common.Specflow.Steps;
using TechTalk.SpecFlow;

namespace Sitecore.Feature.Accounts.Specflow.Steps
{
  [Binding, Scope(Tag = "UI")]
  public class AccountStepsBase : TechTalk.SpecFlow.Steps
  {
    public AccountLocators Site => new AccountLocators();

    public CommonLocators SiteBase => new CommonLocators(this.FeatureContext);

    public AccountSettings Settings => new AccountSettings();


    public MockupOfExternalPageFeature MockupOfExternalPageFeature = new MockupOfExternalPageFeature();


    protected static T GetAnalytycsEntities<T>(string outcomeUrl)
    {
      using (var webClient = new WebClient())
      {
        webClient.Headers.Add(HttpRequestHeader.Accept, "application/json");
        return JsonConvert.DeserializeObject<T>(webClient.DownloadString(outcomeUrl));
      }
    }

    protected Guid GetContactId(string email)
    {
      using (var webClient = new WebClient())
      {
        webClient.Headers.Add(HttpRequestHeader.Accept, "application/json");
        var username = email.Split('@').First();
        var searchResult =
          JsonConvert.DeserializeObject<SearchEntity>(webClient.DownloadString(Settings.SearchContactUrl + username));
        var contactId =
          searchResult.Data.Dataset.ContactSearchResults.First(x => x.PreferredEmailAddress == email).ContactId;
        return contactId;
      }
    }
  }
}