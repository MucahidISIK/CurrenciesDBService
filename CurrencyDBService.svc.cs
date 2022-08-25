using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Net;

namespace CurrenciesDBService
{
    public class CurrencyDBService : ICurrencyDBService
    {
        public string insertCurrencyDetails(CurrencyDetails currencyDetails)
        {
            string DateSQL = currencyDetails.Date_.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string dataBaseAdress = "Server=DESKTOP-SPUVTCQ;Database=CurrencyDetails;User Id=sa;Password=pass;";
            SqlConnection con = new SqlConnection(dataBaseAdress);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Currencies(Currency,Buying,Selling,Date) values(@Currency,@Buying,@Selling,@DateSQL)", con);
            cmd.Parameters.AddWithValue("@Currency", currencyDetails.Currency_);
            cmd.Parameters.AddWithValue("@Buying", currencyDetails.Buying_);
            cmd.Parameters.AddWithValue("@Selling", currencyDetails.Selling_);
            cmd.Parameters.AddWithValue("@DateSQL", DateSQL);

            string Message = "";
            int result = cmd.ExecuteNonQuery();

            if (result == 1)
            {
                Message = currencyDetails.Currency_ + " Details inserted successfully";
            }
            else
            {
                Message = currencyDetails.Currency_ + " Details could not inserted successfully";
            }
            con.Close();
            return Message;
        }

        public string updateDB()
        {
            string message = "";
            WebClient client = new WebClient();
            var response = client.DownloadString("http://hasanadiguzel.com.tr/api/kurgetir");
            CurrencyInfo liveData = CurrencyInfo.FromJson(response);
            var tcmbLiveCurrencyInfo = liveData.TcmbAnlikKurBilgileri;

            for (int i = 0; i < tcmbLiveCurrencyInfo.Length-1; i++)
            {
                CurrencyDetails currencyDetails = new CurrencyDetails();
                
                currencyDetails.Date_ = DateTime.Now;
                currencyDetails.Currency_ = translate(tcmbLiveCurrencyInfo[i].CurrencyName);
                currencyDetails.Buying_ = Convert.ToDouble(tcmbLiveCurrencyInfo[i].ForexBuying);
                currencyDetails.Selling_ = Convert.ToDouble(tcmbLiveCurrencyInfo[i].ForexSelling);

                message = insertCurrencyDetails(currencyDetails);
            }
            return message;
        }

        public List<CurrencyDetails> getCurreny(DateTime selectedDate, string selectedCurrency)
        {
            List<CurrencyDetails> currencyDetailsList = new List<CurrencyDetails>();
            string selectedDateSQL = selectedDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string dataBaseAdress = "Server=DESKTOP-SPUVTCQ;Database=CurrencyDetails;User Id=sa;Password=pass;";
            SqlConnection con = new SqlConnection(dataBaseAdress);
            con.Open();

            if (selectedCurrency == null || selectedCurrency == "TÜM PARA BİRİMLERİ")
            {
                SqlCommand cmd = new SqlCommand("Select * from Currencies where Date = @selectedDateSQL", con);
                cmd.Parameters.AddWithValue("@selectedDateSQL", selectedDateSQL);

                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        CurrencyDetails currencyDetails = new CurrencyDetails();
                        currencyDetails.Currency_ = oReader["Currency"].ToString();
                        currencyDetails.Buying_ = Convert.ToDouble(oReader["Buying"]);
                        currencyDetails.Selling_ = Convert.ToDouble(oReader["Selling"]);
                        currencyDetails.Date_ = Convert.ToDateTime(oReader["Date"].ToString());
                        currencyDetailsList.Add(currencyDetails);
                    }
                }
            }
            else
            {
                SqlCommand cmd = new SqlCommand("Select * from Currencies where Currency = @selectedCurrency AND Date = @selectedDateSQL", con);
                cmd.Parameters.AddWithValue("@selectedCurrency", selectedCurrency);
                cmd.Parameters.AddWithValue("@selectedDateSQL", selectedDateSQL);

                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        CurrencyDetails currencyDetails = new CurrencyDetails();
                        currencyDetails.Currency_ = oReader["Currency"].ToString();
                        currencyDetails.Buying_ = Convert.ToDouble(oReader["Buying"]);
                        currencyDetails.Selling_ = Convert.ToDouble(oReader["Selling"]);
                        currencyDetails.Date_ = Convert.ToDateTime(oReader["Date"].ToString());
                        currencyDetailsList.Add(currencyDetails);
                    }
                }
            }
            con.Close();
            return currencyDetailsList;
        }

        public string translate(string currency)
        {
            switch (currency)
            {

                case "US DOLLAR":
                    currency = "ABD DOLARI";
                    break;
                case "AUSTRALIAN DOLLAR":
                    currency = "AVUSTRALYA DOLARI";
                    break;
                case "DANISH KRONE":
                    currency = "DANİMARKA KRONU";
                    break;
                case "EURO":
                    currency = "EURO";
                    break;
                case "POUND STERLING":
                    currency = "İNGİLİZ STERLİNİ";
                    break;
                case "SWISS FRANK":
                    currency = "İSVİÇRE FRANGI";
                    break;
                case "SWEDISH KRONA":
                    currency = "İSVEÇ KRONU";
                    break;
                case "CANADIAN DOLLAR":
                    currency = "KANADA DOLARI";
                    break;
                case "KUWAITI DINAR":
                    currency = "KUVEYT DİNARI";
                    break;
                case "NORWEGIAN KRONE":
                    currency = "NORVEÇ KRONU";
                    break;
                case "SAUDI RIYAL":
                    currency = "SUUDİ ARABİSTAN RİYALİ";
                    break;
                case "JAPENESE YEN":
                    currency = "JAPON YENİ";
                    break;
                case "BULGARIAN LEV":
                    currency = "BULGAR LEVASI";
                    break;
                case "NEW LEU":
                    currency = "RUMEN LEYİ";
                    break;
                case "RUSSIAN ROUBLE":
                    currency = "RUS RUBLESİ";
                    break;
                case "IRANIAN RIAL":
                    currency = "İRAN RİYALİ";
                    break;
                case "CHINESE RENMINBI":
                    currency = "ÇİN YUANI";
                    break;
                case "PAKISTANI RUPEE":
                    currency = "PAKİSTAN RUPİSİ";
                    break;
                case "QATARI RIAL":
                    currency = "KATAR RİYALİ";
                    break;
                case "SOUTH KOREAN WON":
                    currency = "GÜNEY KORE WONU";
                    break;
                case "AZERBAIJANI NEW MANAT":
                    currency = "AZERBAYCAN YENİ MANATI";
                    break;
                case "UNITED ARAB EMIRATES DIRHAM":
                    currency = "BİRLEŞİK ARAP EMİRLİKLERİ DİRHEMİ";
                    break;
                default:
                    break;
            }

            return currency;
        }
    }
}
