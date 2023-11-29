# otb.techtest
Technical test for On The Beach

# Tasks
* Scaffold solution - DONE
* Add data access for json files - DONE
* Add search service with basic search - DONE
* Add "any" search functionality to search service - DONE
* Refactr - DONE

# What I would do next if I had more time

Implement short caching on the flight and hotel repositories to reduce strain on the database
Implement longer caching on the airport repository as that is data that won't change often
Obviously the secrets for the Airport API should be in configuration not code
If needed it would be trivial to implement independant Flight and Hotel search methods on the HolidaySearchService