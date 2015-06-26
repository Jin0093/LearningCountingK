using CountingKs.Data;
using CountingKs.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace CountingKs.Models
{
    public class ModelFactory
    {

        private UrlHelper _urlHelp;
        private ICountingKsRepository _repo;

        public ModelFactory(HttpRequestMessage request, ICountingKsRepository repo)
        {
            _urlHelp = new UrlHelper(request);
            _repo = repo;
        }
        public FoodModel Create(Food food)
        {
            return new FoodModel()
            {
                URL = _urlHelp.Link("Food", new { foodid = food.Id }),
                Description = food.Description,
                Measure = food.Measures.Select(m=> Create(m))
            };
        }

        public MeasureModel Create(Measure m)
        {
            return new MeasureModel()
            {
                URL = _urlHelp.Link("Measures", new {foodid = m.Food.Id, id = m.Id}),
                Description = m.Description,
                Calories = m.Calories
            };
        }

        public DiaryModel Create(Diary d)
        {
            return new DiaryModel()
            {
                URL = _urlHelp.Link("Diaries", new { diaryid = d.CurrentDate.ToString("yyyy-MM-dd")}),
                CurrentDate = d.CurrentDate
            };
          
        }

        public DiaryEntryModel Create(DiaryEntry entry)
        {
            return new DiaryEntryModel()
            {
                Url = _urlHelp.Link("DiaryEntries", new { diaryid = entry.Diary.CurrentDate.ToString("yyyy-dd-mm"), id = entry })
            };
        }

        public DiaryEntry Parse(DiaryEntryModel model)
        {
            try
            {
                //Model Id
                var entry = new DiaryEntry();
                if(model.Quantity != default(double))
                {
                    entry.Quantity = model.Quantity;
                }


                if (!string.IsNullOrWhiteSpace(model.MeasureUrl))
                {
                    //Measure Id
                    var uri = new Uri(model.MeasureUrl);
                    var measureId = int.Parse(uri.Segments.Last());
                    var measure = _repo.GetMeasure(measureId);
                    entry.Measure = measure;
                    entry.FoodItem = measure.Food;
                }
                return entry;
            }
            catch
            {
                return null;
            }
        }

        public DiarySummaryModel CreateSummary(Diary diary)
        {
            return new DiarySummaryModel()
            {
                DiaryDate = diary.CurrentDate,
                TotalCalories = Math.Round(diary.Entries.Sum(e => e.Measure.Calories * e.Quantity))
            };
        }

        public AuthTokenModel Create(AuthToken authToken)
        {
            return new AuthTokenModel
            {
                Token = authToken.Token,
                ExpirationDate = authToken.Expiration
               
            };
        }

        public MeasureV2eModel Create2(Measure m)
        {
            return new MeasureV2eModel
            {
                URL = _urlHelp.Link("Measures", new { foodid = m.Food.Id, id = m.Id }),
                Description = m.Description,
                Calories = m.Calories,
                Carbohydrates = m.Carbohydrates,
                Cholestrol = m.Cholestrol,
                Fiber = m.Fiber,
                Iron = m.Iron,
                Protein = m.Protein,
                SaturatedFat = m.SaturatedFat,
                Sodium = m.Sodium,
                Sugar = m.Sugar,
                TotalFat = m.TotalFat
            };
        }
    }
}