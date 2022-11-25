using DoAnChuyenNganh_SQLServer.Interface;
using DoAnChuyenNganh_SQLServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.Expressions;

namespace DoAnChuyenNganh_SQLServer.Service
{
    public class NBCFService : INBCF
    {
        GearShopDataContext _context = new GearShopDataContext();
        public string[,] GetData()
        {
            int user = _context.Customers.Count()+1;
            int product = _context.Products.Count()+1;
            var listCustomer = _context.Customers.Where(s=>s.Status != false).ToList();
            var listProduct = _context.Products.Where(s => s.Status != false).ToList();
            var listRatting = _context.FeedBacks;
            string[,] matrix = new string[product, user];
            for(int i=1; i < user; i++)
            {
                matrix[0, i] = listCustomer[i-1].CustomerID;
            }
            for (int i = 1; i < product; i++)
            {
                matrix[i, 0] = listProduct[i - 1].ProductID;
            }
            for (int i = 1; i < user; i++)
            {
                for (int j = 1; j < product; j++)
                {
                    var ratting = listRatting.Where(s => s.CustomerID == matrix[0, i] && s.ProductID == matrix[j, 0]).FirstOrDefault();
                    if (ratting == null)
                    {
                        matrix[j, i] = null;
                    }
                    else
                    {
                        matrix[j, i] = ratting.Rating.ToString();
                    }
                }
            }
            return matrix;
        }
        // tinh trung binh tung cot
        public float[] ColumnAverage(string[,] data)
        {
            float[] avgRatting = new float[data.GetLength(1)];
            for(int i =1;i< data.GetLength(1); i++)
            {
                
                int sum = 0;
                avgRatting[i] = 0;
                for(int j = 1; j < data.GetLength(0); j++)
                {
                    if (data[j, i] != null)
                    {
                        sum += 1;
                    avgRatting[i] += float.Parse(data[j, i], CultureInfo.InvariantCulture.NumberFormat);
                    }
                }
                avgRatting[i] /= sum;
            }
            return avgRatting;
        } 

        // binh thuong hoa du lieu
        public string[,] Normalized()
        {
            string[,] data = GetData();
            var avgColumn = ColumnAverage(data);
            for(int i = 1; i < data.GetLength(0);i++)
            {
                for(int j = 1; j < data.GetLength(1);j++)
                {
                    if (data[i,j]== null)
                    {
                        data[i, j] = 0.ToString();
                    }
                    else
                    {
                        data[i,j] = (float.Parse(data[i, j], CultureInfo.InvariantCulture.NumberFormat) - avgColumn[j]).ToString();
                    }
                }
            }

            return data;
        }
        //lap ma tran tinh do tuong quan cua cac user
        public string[,] UserSimilarity()
        {
            var data = Normalized();

            var listUser = _context.Customers.ToList();
            string[,] similarity = new string[data.GetLength(1), data.GetLength(1)];
            for(int i = 1; i < data.GetLength(1); i++)
            {
                similarity[0, i] = listUser[i-1].CustomerID;
                similarity[i, 0] = listUser[i-1].CustomerID;
            }
             for(int i =1;i< data.GetLength(1); i++)
            {
                for(int j = 1; j < data.GetLength(1); j++)
                {
                    similarity[i,j]= Cotst(i,j,data).ToString();
                }
            }
            return similarity;
        }
        //goi y san pham
        public string[,] GuessNormal()
        {
            string[,] normallize = Normalized();
            string[,] similary = UserSimilarity();
            
            for (int i=0;i < normallize.GetLength(0); i++)
            {
                for(int j =0; j < normallize.GetLength(1); j++)
                {
                    if (normallize[i,j] == 0.ToString())
                    {
                        normallize[i, j] = Interest(normallize, similary, i, j).ToString();
                    }
                }
            }
            return normallize;
        }

        // recomment for you

        public List<string> RecommentForYou(string customerID)
        {
            string[,] recomment = GuessNormal();
            int Getitem = -1;
            for(int i = 0; i < recomment.GetLength(1); i++)
            {
                if(recomment[0,i] == customerID)
                {
                    Getitem = i;
                    break;
                }
            }
           for(int i =1;i< recomment.GetLength(0); i++)
            {
                for(int j=i+1; j < recomment.GetLength(0); j++)
                {
                    if (float.Parse(recomment[i,Getitem])< float.Parse(recomment[j, Getitem]))
                    {
                        //change value
                        var a = recomment[i,Getitem];
                        recomment[i,Getitem] = recomment[j, Getitem];
                        recomment[j,Getitem] = a;
                        //change item
                        var b = recomment[i, 0];
                        recomment[i, 0] = recomment[j, 0];
                        recomment[j,0]=b;
                    }
                }
            }
            List<string> listProduct = new List<string>();
            for(int i = 1; i < recomment.GetLength(0); i++)
            {
                listProduct.Add(recomment[i,0]);
            }
            return listProduct;
        }


        //-----------------------------------function-----------------------------------------------
        //get cost user
        public float Cotst(int a, int b, string[,] arr)
        {
            if (a == b)
            {
                return 1;
            }
            else
            {
                float scalar = 0;
                float lenghtA = 0;
                float lenghtB = 0;
                for (int i = 1; i < arr.GetLength(0); i++)
                {
                    scalar +=float.Parse(arr[i,a]) * float.Parse(arr[i,b]);
                    lenghtA += float.Parse(arr[i, a]) * float.Parse(arr[i,a]);
                    lenghtB += float.Parse(arr[i, b]) * float.Parse(arr[i, b]);
                }
                return (float)scalar/(float)Math.Sqrt(lenghtB * lenghtA);
            }
        }

        //get user ratting item
        public float Interest(string[,] normallize, string[,] similary, int j, int user)
        {
            List<int> userRatings = new List<int>();
            List<float> userRatingsVal = new List<float>();
            for(int i = 1; i < normallize.GetLength(1); i++)
            {
                if (normallize[j, i] != 0.ToString())
                {
                    userRatings.Add(i);
                    userRatingsVal.Add(float.Parse(normallize[j,i]));
                }
            }
            List<float> userSimilary = new List<float>();
            foreach (int i in userRatings)
            {
                userSimilary.Add(float.Parse(similary[user, i]));
            }

            float guessRatting = 0;
            float avgSimilary = 0;
            for(int i = 0; i < userRatings.Count(); i++)
            {
                guessRatting += userRatingsVal[i] * userSimilary[i];
                avgSimilary += Math.Abs(userSimilary[i]);
            }
            float guess = guessRatting / avgSimilary;
            return guess;
        }

    }
}