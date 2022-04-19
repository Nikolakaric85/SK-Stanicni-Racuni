using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace SK_Stanicni_Racuni.Controllers
{
    public class CurrencyToTextController : Controller
    {
        public ActionResult CurrencyToText(string data)
        {
            if (data == null)
            {
                data = "0.00";
            }

            string[] array = data.Split('.');
            string tempBroj = array[0].Replace(",", "");
            int maxDuzina = 9;
            int duzina = tempBroj.Length;
            int razlika = maxDuzina - duzina;

            StringBuilder brojBuilder = new StringBuilder();
            brojBuilder.Append(tempBroj);

            for (int i = 0; i < razlika; i++)
            {
                brojBuilder.Insert(0, "0");
            }
            
            StringBuilder res = new StringBuilder();

            if (brojBuilder.Length == 9)
            {
                string broj = brojBuilder.ToString();

                string textM1 = string.Empty;
                string textM2 = string.Empty;
                string textM3 = string.Empty;

                string textT1 = string.Empty;
                string textT2 = string.Empty;
                string textT3 = string.Empty;

                string textS1 = string.Empty;
                string textS2 = string.Empty;
                string textS3 = string.Empty;


                if (broj.Substring(0, 1) != "0")        //stotine miliona
                {
                    if (broj.Substring(0, 1) == "1")
                    {
                        textM1 = "sto";
                    }
                    else if (broj.Substring(0, 1) == "2")
                    {
                        textM1 = "dvesta";
                    }
                    else if (broj.Substring(0, 1) == "3")
                    {
                        textM1 = "trista";
                    }
                    else if (broj.Substring(0, 1) == "4")
                    {
                        textM1 = "četiristo";
                    }
                    else if (broj.Substring(0, 1) == "5")
                    {
                        textM1 = "petsto";
                    }
                    else if (broj.Substring(0, 1) == "6")
                    {
                        textM1 = "šesto";
                    }
                    else if (broj.Substring(0, 1) == "7")
                    {
                        textM1 = "sedamsto";
                    }
                    else if (broj.Substring(0, 1) == "8")
                    {
                        textM1 = "osamsto";
                    }
                    else if (broj.Substring(0, 1) == "9")
                    {
                        textM1 = "devetsto";
                    }
                }
                else
                {
                    textM1 = string.Empty;
                }

                if (broj.Substring(1, 1) != "0")
                {
                    if (broj.Substring(1, 1) == "1")
                    {
                        if (broj.Substring(2, 1) == "0")
                        {
                            textM2 = "deset";
                        }
                        else if (broj.Substring(2, 1) == "1")
                        {
                            textM2 = "jedanaest";
                        }
                        else if (broj.Substring(2, 1) == "2")
                        {
                            textM2 = "dvanaest";
                        }
                        else if (broj.Substring(2, 1) == "3")
                        {
                            textM2 = "trinaest";
                        }
                        else if (broj.Substring(2, 1) == "4")
                        {
                            textM2 = "četrnaest";
                        }
                        else if (broj.Substring(2, 1) == "5")
                        {
                            textM2 = "petnaest";
                        }
                        else if (broj.Substring(2, 1) == "6")
                        {
                            textM2 = "šesnaest";
                        }
                        else if (broj.Substring(2, 1) == "7")
                        {
                            textM2 = "sedamnaest";
                        }
                        else if (broj.Substring(2, 1) == "8")
                        {
                            textM2 = "osamnest";
                        }
                        else if (broj.Substring(2, 1) == "9")
                        {
                            textM2 = "devetnaest";
                        }
                    }
                    else if (broj.Substring(1, 1) == "2")
                    {
                        textM2 = "dvadeset";
                    }
                    else if (broj.Substring(1, 1) == "3")
                    {
                        textM2 = "trideset";
                    }
                    else if (broj.Substring(1, 1) == "4")
                    {
                        textM2 = "četrdeset";
                    }
                    else if (broj.Substring(1, 1) == "5")
                    {
                        textM2 = "pedeset";
                    }
                    else if (broj.Substring(1, 1) == "6")
                    {
                        textM2 = "šezdeset";
                    }
                    else if (broj.Substring(1, 1) == "7")
                    {
                        textM2 = "sedamdeset";
                    }
                    else if (broj.Substring(1, 1) == "8")
                    {
                        textM2 = "osamdeset";
                    }
                    else if (broj.Substring(1, 1) == "9")
                    {
                        textM2 = "devedeset";
                    }
                    else
                    {
                        textM2 = string.Empty;
                    }
                }
                else
                {
                    if (broj.Substring(2, 1) == "1")
                    {
                        textM3 = "jedan";
                    }
                    else if (broj.Substring(2, 1) == "2")
                    {
                        textM3 = "dva";
                    }
                    else if (broj.Substring(2, 1) == "3")
                    {
                        textM3 = "tri";
                    }
                    else if (broj.Substring(2, 1) == "4")
                    {
                        textM3 = "četiri";
                    }
                    else if (broj.Substring(2, 1) == "5")
                    {
                        textM3 = "pet";
                    }
                    else if (broj.Substring(2, 1) == "6")
                    {
                        textM3 = "šest";
                    }
                    else if (broj.Substring(2, 1) == "7")
                    {
                        textM3 = "sedam";
                    }
                    else if (broj.Substring(2, 1) == "8")
                    {
                        textM3 = "osam";
                    }
                    else if (broj.Substring(2, 1) == "9")
                    {
                        textM3 = "devet";
                    }

                }

                if (broj.Substring(1, 1) != "1")
                {
                    if (broj.Substring(2, 1) == "1")
                    {
                        textM3 = "jedan";
                    }
                    else if (broj.Substring(2, 1) == "2")
                    {
                        textM3 = "dva";
                    }
                    else if (broj.Substring(2, 1) == "3")
                    {
                        textM3 = "tri";
                    }
                    else if (broj.Substring(2, 1) == "4")
                    {
                        textM3 = "četiri";
                    }
                    else if (broj.Substring(2, 1) == "5")
                    {
                        textM3 = "pet";
                    }
                    else if (broj.Substring(2, 1) == "6")
                    {
                        textM3 = "šest";
                    }
                    else if (broj.Substring(2, 1) == "7")
                    {
                        textM3 = "sedam";
                    }
                    else if (broj.Substring(2, 1) == "8")
                    {
                        textM3 = "osam";
                    }
                    else if (broj.Substring(2, 1) == "9")
                    {
                        textM3 = "devet";
                    }
                }

                StringBuilder resM = new StringBuilder();

                if (string.IsNullOrEmpty(textM1) && string.IsNullOrEmpty(textM2) && string.IsNullOrEmpty(textM3))
                {
                    resM.Append(string.Empty);
                }
                else if (textM3 == "jedan" && string.IsNullOrEmpty(textM1) && string.IsNullOrEmpty(textM2))
                {
                    resM.Append(textM3 + "milion");
                }
                else
                {
                    resM.Append(textM1 + textM2 + textM3 + "miliona");
                }


                // kraj za milione
                // pocetak hiljade


                if (broj.Substring(3, 1) != "0")
                {
                    if (broj.Substring(3, 1) == "1")
                    {
                        textT1 = "sto";
                    }
                    else if (broj.Substring(3, 1) == "2")
                    {
                        textT1 = "dvesta";
                    }
                    else if (broj.Substring(3, 1) == "3")
                    {
                        textT1 = "trista";
                    }
                    else if (broj.Substring(3, 1) == "4")
                    {
                        textT1 = "četiristo";
                    }
                    else if (broj.Substring(3, 1) == "5")
                    {
                        textT1 = "petsto";
                    }
                    else if (broj.Substring(3, 1) == "6")
                    {
                        textT1 = "šesto";
                    }
                    else if (broj.Substring(3, 1) == "7")
                    {
                        textT1 = "sedamsto";
                    }
                    else if (broj.Substring(3, 1) == "8")
                    {
                        textT1 = "osamsto";
                    }
                    else if (broj.Substring(3, 1) == "9")
                    {
                        textT1 = "devetsto";
                    }
                    else
                    {
                        textT1 = string.Empty;
                    }
                }

                if (broj.Substring(4, 1) != "0")
                {
                    if (broj.Substring(4, 1) == "1")
                    {
                        if (broj.Substring(5, 1) == "1")
                        {
                            textT2 = "jedanaest";
                        }
                        else if (broj.Substring(5, 1) == "2")
                        {
                            textT2 = "dvanaest";
                        }
                        else if (broj.Substring(5, 1) == "3")
                        {
                            textT2 = "trinaest";
                        }
                        else if (broj.Substring(5, 1) == "4")
                        {
                            textT2 = "četrnaest";
                        }
                        else if (broj.Substring(5, 1) == "5")
                        {
                            textT2 = "petnaest";
                        }
                        else if (broj.Substring(5, 1) == "6")
                        {
                            textT2 = "šesnaest";
                        }
                        else if (broj.Substring(5, 1) == "7")
                        {
                            textT2 = "sedamnaest";
                        }
                        else if (broj.Substring(5, 1) == "8")
                        {
                            textT2 = "osamnaest";
                        }
                        else if (broj.Substring(5, 1) == "9")
                        {
                            textT2 = "devetnaest";
                        }
                        else if (broj.Substring(5, 1) == "0")
                        {
                            textT2 = "deset";
                        }
                    }
                    else if (broj.Substring(4, 1) == "2")
                    {
                        textT2 = "dvadeset";
                    }
                    else if (broj.Substring(4, 1) == "3")
                    {
                        textT2 = "trideset";
                    }
                    else if (broj.Substring(4, 1) == "4")
                    {
                        textT2 = "četrdeset";
                    }
                    else if (broj.Substring(4, 1) == "5")
                    {
                        textT2 = "pedeset";
                    }
                    else if (broj.Substring(4, 1) == "6")
                    {
                        textT2 = "šezdeset";
                    }
                    else if (broj.Substring(4, 1) == "7")
                    {
                        textT2 = "sedamdeset";
                    }
                    else if (broj.Substring(4, 1) == "8")
                    {
                        textT2 = "osamdeset";
                    }
                    else if (broj.Substring(4, 1) == "9")
                    {
                        textT2 = "devedeset";
                    }
                }

                if (broj.Substring(4, 1) != "1")
                {
                    if (broj.Substring(5, 1) == "1")
                    {
                        textT3 = "jedna";
                    }
                    else if (broj.Substring(5, 1) == "2")
                    {
                        textT3 = "dve";
                    }
                    else if (broj.Substring(5, 1) == "3")
                    {
                        textT3 = "tri";
                    }
                    else if (broj.Substring(5, 1) == "4")
                    {
                        textT3 = "četiri";
                    }
                    else if (broj.Substring(5, 1) == "5")
                    {
                        textT3 = "pet";
                    }
                    else if (broj.Substring(5, 1) == "6")
                    {
                        textT3 = "šest";
                    }
                    else if (broj.Substring(5, 1) == "7")
                    {
                        textT3 = "sedam";
                    }
                    else if (broj.Substring(5, 1) == "8")
                    {
                        textT3 = "osam";
                    }
                    else if (broj.Substring(5, 1) == "9")
                    {
                        textT3 = "devet";
                    }
                    else
                    {
                        textT3 = string.Empty;
                    }
                }

                StringBuilder resT = new StringBuilder();

                if (string.IsNullOrEmpty(textT1) && string.IsNullOrEmpty(textT2) && string.IsNullOrEmpty(textT3))
                {
                    resT.Append(string.Empty);
                }
                else if ((textT3 == "dve" || textT3 == "tri" || textT3 == "četiri"))
                {
                    resT.Append(textT1 + textT2 + textT3 + "hiljade");
                }
                else
                {
                    resT.Append(textT1 + textT2 + textT3 + "hiljada");
                }

                //kraj za hiljade
                //pocetak za stotine

                if (broj.Substring(6, 1) != "0")
                {
                    if (broj.Substring(6, 1) == "1")
                    {
                        textS1 = "sto";
                    }
                    else if (broj.Substring(6, 1) == "2")
                    {
                        textS1 = "dvesta";
                    }
                    else if (broj.Substring(6, 1) == "3")
                    {
                        textS1 = "trista";
                    }
                    else if (broj.Substring(6, 1) == "4")
                    {
                        textS1 = "četiristo";
                    }
                    else if (broj.Substring(6, 1) == "5")
                    {
                        textS1 = "petsto";
                    }
                    else if (broj.Substring(6, 1) == "6")
                    {
                        textS1 = "šesto";
                    }
                    else if (broj.Substring(6, 1) == "7")
                    {
                        textS1 = "sedamsto";
                    }
                    else if (broj.Substring(6, 1) == "8")
                    {
                        textS1 = "osamsto";
                    }
                    else if (broj.Substring(6, 1) == "9")
                    {
                        textS1 = "devetsto";
                    }
                    else
                    {
                        textS1 = string.Empty;
                    }
                }

                if (broj.Substring(7, 1) != "0")
                {
                    if (broj.Substring(7, 1) == "1")
                    {
                        if (broj.Substring(8, 1) == "1")
                        {
                            textS2 = "jedanaest";
                        }
                        else if (broj.Substring(8, 1) == "2")
                        {
                            textS2 = "dvanaest";
                        }
                        else if (broj.Substring(8, 1) == "3")
                        {
                            textS2 = "trinaest";
                        }
                        else if (broj.Substring(8, 1) == "4")
                        {
                            textS2 = "četrnaest";
                        }
                        else if (broj.Substring(8, 1) == "5")
                        {
                            textS2 = "petnaest";
                        }
                        else if (broj.Substring(8, 1) == "6")
                        {
                            textS2 = "šesnaest";
                        }
                        else if (broj.Substring(8, 1) == "7")
                        {
                            textS2 = "sedamnaest";
                        }
                        else if (broj.Substring(8, 1) == "8")
                        {
                            textS2 = "osamnest";
                        }
                        else if (broj.Substring(8, 1) == "9")
                        {
                            textS2 = "devetnaest";
                        }
                        else
                        {
                            textS2 = "deset";
                        }
                    }

                    else if (broj.Substring(7, 1) == "2")
                    {
                        textS2 = "dvadeset";
                    }
                    else if (broj.Substring(7, 1) == "3")
                    {
                        textS2 = "trideset";
                    }
                    else if (broj.Substring(7, 1) == "4")
                    {
                        textS2 = "četrdeset";
                    }
                    else if (broj.Substring(7, 1) == "5")
                    {
                        textS2 = "pedeset";
                    }
                    else if (broj.Substring(7, 1) == "6")
                    {
                        textS2 = "šezdeset";
                    }
                    else if (broj.Substring(7, 1) == "7")
                    {
                        textS2 = "sedamdeset";
                    }
                    else if (broj.Substring(7, 1) == "8")
                    {
                        textS2 = "osamdeset";
                    }
                    else if (broj.Substring(7, 1) == "9")
                    {
                        textS2 = "devedeset";
                    }
                    else
                    {
                        textS2 = "";
                    }
                }

                if (broj.Substring(7, 1) != "1")
                {
                    if (broj.Substring(8, 1) == "1")
                    {
                        textS3 = "jedan";
                    }
                    else if (broj.Substring(8, 1) == "2")
                    {
                        textS3 = "dva";
                    }
                    else if (broj.Substring(8, 1) == "3")
                    {
                        textS3 = "tri";
                    }
                    else if (broj.Substring(8, 1) == "4")
                    {
                        textS3 = "četiri";
                    }
                    else if (broj.Substring(8, 1) == "5")
                    {
                        textS3 = "pet";
                    }
                    else if (broj.Substring(8, 1) == "6")
                    {
                        textS3 = "šest";
                    }
                    else if (broj.Substring(8, 1) == "7")
                    {
                        textS3 = "sedam";
                    }
                    else if (broj.Substring(8, 1) == "8")
                    {
                        textS3 = "osam";
                    }
                    else if (broj.Substring(8, 1) == "9")
                    {
                        textS3 = "devet";
                    }
                    else
                    {
                        textS3 = string.Empty;
                    }
                }

                StringBuilder resS = new StringBuilder();
                resS.Append(textS1 + textS2 + textS3);

                if (resM.Length > 0 || resT.Length > 0 || resS.Length > 0)
                {
                    res.Append(resM);
                    res.Append(resT);
                    res.Append(resS);
                    res.Append(" dinara i " + array[1] + "/100");
                }
                else
                {
                    res.Append(string.Empty);
                }
            }
            else if(brojBuilder.Length > 9)
            {
                res.Append(string.Empty);
            }
          
            return Json(res.ToString());
        }
    }
}
