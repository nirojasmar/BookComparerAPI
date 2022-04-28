# BookComparerAPI
API that has the purpose of providing a lenghty book deals cathalog for the BookComparer web-services. Supported by Swagger and ScrapySharp 3.0
## GetBookList JSON

```json
[
  {
    "isbn": 9780062315005,
    "name": "The Alchemist, 25th Anniversary: A Fable About Following Your Dream                                                                                                                                     ",
    "author": "Paulo Coelho                                      ",
    "editor": "N/A                                               ",
    "language": "Ingles         ",
    "format": "Pasta blanda                  ",
    "url": "www.amazon.com/-/es/Paulo-Coelho/dp/0062315005/ref=sr_1_21?qid=1651178040&amp;refinements=p_n_feature_browse-bin%3A2656022011&amp;rnid=618072011&amp;s=books&amp;sr=1-21",
    "priceDates": [
      {
        "date": "2022-04-28T15:34:50.767",
        "price": 8.89,
        "store": "Amazon                        "
      }
    ]
  },
  {
    "isbn": 9780063078902,
    "name": "Bridgerton [TV Tie-in] (Bridgertons Book 1)                                                                                                                                                             ",
    "author": "Julia Quinn                                       ",
    "editor": "N/A                                               ",
    "language": "Ingles         ",
    "format": "Pasta blanda                  ",
    "url": "www.amazon.com/-/es/Julia-Quinn/dp/0063078902/ref=sr_1_31?qid=1650924490&amp;refinements=p_n_feature_browse-bin%3A2656022011&amp;rnid=618072011&amp;s=books&amp;sr=1-31",
    "priceDates": [
      {
        "date": "2022-04-25T17:42:15.027",
        "price": 10.99,
        "store": "Amazon                        "
      },
      {
        "date": "2022-04-25T22:18:52.7",
        "price": 10.99,
        "store": "Amazon                        "
      }
    ]
  }
]
```

## ChangeLog
- 28/04/2022 : API Completed :)
- 27/04/2022 : DAO now recovers PriceDates for API Operations (Experimental)
- 25/04/2022 : DAO Fixed Types and use of Scraper, BL and Amazon Prices are properly added to the DB
- 24/04/2022 : Change Controllers to use DAO and not the Scraper directly (Experimental)
- 23/04/2022 : "Fixed" User-Agent Block (Random)
- 22/04/2022 : Change Visibility to Public
- 8/04/2022 : Created new branch for database development & testing

### This Program is for educational/demonstration purposes only. 
