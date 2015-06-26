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
                Links = new List<LinkModel>()
                {
                    CreateLink(_urlHelp.Link("Measures", new {foodid = m.Food.Id, id = m.Id}), "POST")
                },
                Description = m.Description,
                Calories = m.Calories
            };
        }

        public DiaryModel Create(Diary d)
        {
            return new DiaryModel()
            {
                Links = new List<LinkModel>()
                {
                    CreateLink(_urlHelp.Link("Diaries", new { diaryid = d.CurrentDate.ToString("yyyy-MM-dd")}),
                        "self")
                },
                CurrentDate = d.CurrentDate,
                Entries = d.Entries.Select(e=>Create(e))
            };
          
        }

        public LinkModel CreateLink(string href, string rel, string method = "GET", bool isTemplated = false)
        {
            return new LinkModel()
            {
                Href = href,
                Rel = rel,
                Method = method,
                IsTemplated = isTemplated;
            }
        }

        public DiaryEntryModel Create(DiaryEntry entry)
        {
            return new DiaryEntryModel()
            {
                Url = _urlHelp.Link("DiaryEntries", new { diaryid = entry.Diary.CurrentDate.ToString("yyyy-dd-mm"), id = entry })
            };
        }

        public DiaryEntry Parse(DiaryModel model)
        {
            try
            {
                var entity = new Diary();
                var selfLink = model.Links.Where(l=> l.Rel == "self").FirstOrDefault();

                if(selfLink !=null && !string.IsNullOrWhiteSpace(selfLink.Href))
                {
                    var uri = new Uri(selfLink.Href);
                    entity.Id = int.Parse(uri.Segments.Last());
                }

                entity.CurrentDate = model.CurrentDate;

                if (model.Entries !=null)
                {
                    foreach (var entry in model.Entries) entity.Entries.Add(model.Entries);
                }

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