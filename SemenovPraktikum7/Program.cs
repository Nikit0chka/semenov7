using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Praktikum7
{
    internal class CookBook
    {
        private Dictionary<string, List<string[]>> _cookBookDic;

        public CookBook(string pathToFile)
        {
            _cookBookDic = ConvertStringsToDic(ReadFileToStrings(pathToFile));
        }

        public List<string[]> GetShopListForQuantityPersonsByDishes(List<string> dishes, int quantity)
        {
            List<string[]> shopList = new List<string[]>();

            foreach (var dish in dishes)
            {
                if (!_cookBookDic.ContainsKey(dish))
                    throw new Exception("Cookbook is not contain this dish");

                foreach (var ingredients in _cookBookDic[dish])
                {
                    foreach (var word in ingredients)
                        if (double.TryParse(word, out double count))
                            ingredients[Array.IndexOf(ingredients, word)] = (count * quantity).ToString();
                    shopList.Add(ingredients);
                }
            }

            return SortShopList(shopList);
        }

        private List<string[]> SortShopList(List<string[]> shopListInput)
        {
            List<string> ingredientsNamesList = new List<string>();

            foreach (var ingredient in shopListInput)
                ingredientsNamesList.Add(ingredient[0]);

            var ingredientsNamesSet = new HashSet<string>();
            for (int i = 0; i < ingredientsNamesList.Count; i++)
            {
                if (!ingredientsNamesSet.Add(ingredientsNamesList[i]))
                {
                    double value = 0;

                    foreach (var word in shopListInput[ingredientsNamesList.IndexOf(ingredientsNamesList[i])])
                        if (double.TryParse(word, out double asd))
                            value = asd;

                    shopListInput.RemoveAt(ingredientsNamesList.IndexOf(ingredientsNamesList[i]));
                    ingredientsNamesList.Remove(ingredientsNamesList[i]);

                    int indexOfNextCurrentIngredient = ingredientsNamesList.IndexOf(ingredientsNamesList[--i]);

                    foreach (var word in shopListInput[indexOfNextCurrentIngredient])
                        if (double.TryParse(word, out double znachenie))
                            shopListInput[indexOfNextCurrentIngredient][Array.IndexOf(shopListInput[indexOfNextCurrentIngredient], word)] = (znachenie + value).ToString();
                }
            }
            return shopListInput;
        }

        private string[] ReadFileToStrings(string path)
        {
            string[] recieps = new string[] { };

            try { recieps = File.ReadAllLines(path); }
            catch (Exception e) { Console.WriteLine("Exception: " + e.Message); }

            return recieps;
        }

        private Dictionary<string, List<string[]>> ConvertStringsToDic(string[] inputRecieps)
        {
            string key = inputRecieps[0];

            Dictionary<string, List<string[]>> ReciepsDic = new Dictionary<string, List<string[]>>();

            for (int i = 1; i < inputRecieps.Length; i++)
            {
                List<string[]> ingredients = new List<string[]>();
                int valueOfIngredients = Convert.ToInt32(inputRecieps[i]); i++;

                for (int j = 0; j < valueOfIngredients; j++, i++)
                    ingredients.Add(inputRecieps[i].Split('|'));

                ReciepsDic.Add(key, ingredients);

                if (i + 1 < inputRecieps.Length)
                {
                    i++;
                    key = inputRecieps[i];
                }
            }

            return ReciepsDic;
        }
    }

    internal class SomeFile
    {
        private string _directivePath;

        public SomeFile(string directivePath)
        {
            _directivePath = directivePath;
        }

        private string[] GetPathesToFilesFromDirective()
        {
            string[] pathes = new string[] { };

            try { pathes = Directory.GetFiles(_directivePath); }
            catch (Exception e) { Console.WriteLine("Exception: " + e.Message); }

            return pathes;
        }
        private void CreateResultFile(Dictionary<string, string[]> resultDic)
        {
            try
            {
                StreamWriter sw = new StreamWriter(Path.Combine(_directivePath, "result.txt"), true);

                foreach (var reslut in resultDic)
                {
                    sw.WriteLine(reslut.Key);
                    foreach (var str in reslut.Value)
                        sw.WriteLine(str);
                }

                sw.Close();
            }
            catch (Exception e) { Console.WriteLine("Exception: " + e.Message); }
        }

        public void CompareFiles()
        {
            string[] pathes = GetPathesToFilesFromDirective();
            if (pathes == null)
                throw new Exception("Current directive is empty");


            Dictionary<string, string[]> filesContentDic = new Dictionary<string, string[]>();

            foreach (var path in pathes)
                filesContentDic.Add(Path.GetFileName(path), File.ReadAllLines(path));

            filesContentDic = filesContentDic.OrderBy(x => x.Value.Length).ToDictionary(x => x.Key, x => x.Value);
            CreateResultFile(filesContentDic);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            const string PATH_DISHES_LIST = @"C:\Users\voron\OneDrive\Рабочий стол\SemenovPraktikum7\SemenovPraktikum7\DishNames.txt";
            const string PATH_FILE_DIRECTIVE = @"C:\\Users\\voron\\OneDrive\\Рабочий стол\\SemenovPraktikum7\\SemenovPraktikum7\Files";
            CookBook dish1 = new CookBook(PATH_DISHES_LIST);
            SomeFile asd = new SomeFile(PATH_FILE_DIRECTIVE);
        }
    }
}