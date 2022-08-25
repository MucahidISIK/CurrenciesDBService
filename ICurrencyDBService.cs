using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CurrenciesDBService
{
    [ServiceContract]
    public interface ICurrencyDBService
    {
        [OperationContract]
        string insertCurrencyDetails(CurrencyDetails currencyDetails);

        [OperationContract]
        List<CurrencyDetails> getCurreny(DateTime day, string currency);

        [OperationContract]
        string updateDB();

        [OperationContract]
        string translate(string currency);
    }
}