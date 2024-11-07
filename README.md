# Swagger URL
https://elections.appbuilder.dev/swagger/index.html

This project provides an API for accessing and visualizing data for U.S. elections over the years 1940 - 2024. 
**Note: The results for 2024 are predictions based on available data sources and do not reflect the actual election outcome. 
Source for 2024 predictions: The Economist newspaper as of September 30, 2024**
https://www.economist.com/interactive/us-2024-election/prediction-model/president

## Technologies
- **ASP.NET Core Web API**
- **Docker**
- **Newtonsoft.Json**
- **Swagger**

## Data Source
The election data is sourced from publicly available datasets and historical election records. For the 2024 predictions, data from credible prediction models has been used. The 2024 results are subject to change and are not be considered final.

## Endpoints
- **`GET /years`**  
- **`GET /year/{year}`** 
- **`GET /states-abbreviation`**  
- **`GET /state/{abbreviation}`**  
- **`GET /democratic-candidate/{year}`**  
- **`GET /republican-candidate/{year}`** 
- **`GET /electoral-votes/{year}`**  
- **`GET /popular-votes/{year}`**  
- **`GET /electoral-votes/{year}/{party}`**  
- **`GET /popular-votes/{year}/{party}`** 
- **`GET /popular-votes/{year}/by-state`** 
- **`GET /votes/{year}/by-candidate`** 

## Docker instructions
