# WebApi
Aby uruchomić aplikacje to gdy się pobierze z Githuba, to trzeba ustawić połączenie z serverem MS_SQL, uruchomić aplikacje, użyć endpointa api/file -> najpierw pobiera 3 pliki CSV do pliku FilesCSV, potem pobiera z nich odpowiednie rekordy do bazy danych. Następnie powinien automatycznie wykryć brak połączenia z daną bazą, więc tworzy nową bazę danych "ProjectWebApiDb", potem tworzy 3 puste tablice: Products, Prices, Inventories, później je wypełnia danymi rekordami. Jeśli tak się nie połączy to użyć sposób poniższy z komendą update-database.

Aby uruchomić aplikacje trzeba pobrać z githuba, ustawić połączenie z bazą danych MS_SQL wartość ProductsDbContext z appsettings.Developer.json.
Potem wejść w Visual Studio COmmunity 2022 -> Tools-> Nuget Package Manager -> Package Console Manager i wpisać do terminala update-database. Jeśli nie ma migracji lub nie działa, to usunąć plik Migrations z projektu, następnie w treminalu add-migration Init, potem update-database. Za pomocą tych komend tworzy się nowa baza danych ProjectWebApiDb + 3 puste tablice Products, Prices, Inventories w MS_SQL za pomocą Entity Framework.
Gdy się uruchomi aplikacje, otworzy się za pomocą Swaggera jako dokumentacja WEB API.
Aplikacja ma 5 endpointów:
1. GET api/file -> gdy nie ma plików excelowych , to najpierw tworzy katalog FilesCSV -> potem ściąga 3 pliki z sieci i tworzy w katalogu FilesCSV 3 pliki : Products.csv, Prices.csv, Products.csv, potem pobiera rekordy z pliku csv Products -> sprawdza wartość is_wire jeśli jest 0 to nie jest kablem i sprawdza wartość shipping = 24h, to te rekordy pobiera i tworzy nowa tabele Products z 2 kolumnami + Id : Id, Shopping, IsWire
. Potem tak samo pobiera dane z pliku Inventory.scv i sprawdza Shipping == 24H, i te rekordy zapisuje do nowej tabeli Inventories z kolumnami: id , Shopping. Potem pobiera rekordy z pliku Prices.csv , wszystkie z kolumnami: id, NetProductPrice -> cena netto, NettProductPriceAfterDiscountForProductLogisticUnit -> cena netto po rabacie dla jednostki do nowej tabeli Prices.

2. GET api/file/details -> trzeba podać jako parametr nr SKU -> i wyszuka z aktualnych list Produktów, Prices, Inventories czy jest dany produkt, jeśli nie ma takiego producktu to informacja aby wprowadzic prawidlowy nr SKU który jest na liscie i bazie danej. Jeśli znajdzie to pokaże wynik: 
- name: nazwa produktu -> a.
- EAN: numer ean -> b.
- producerName: imie producenta -> c.
- category: kategoria -> d.
- defaultImage: url do zdjecia produktu -> e.
- available: stan magazynowy -> f. -> sprawdza czy jest produkt dostępny do zamówienia, czy jest dostępny na stanie, jak tak to 1
- SKU: jednostka logistyczna produktu -> g. -> chyba tak, bo nie jestem pewien czy dobrze zrozumiałem.
- shippingCost: koszt dostawy -> i.
- nettProductPrice i nettPriceAfterDiscountForProductLogistucUnit: cena zakupu netto normal i po znizce dla jednostki -> h.

3. GET api/inventory -> pobiera i wyswietla wszystkie Inventories w bazie danych, można przeglądać na jakieś ma być aktualnej stronie. Trzeba podać PageNumber -> 1 do więcej, PageSize -> 5, 10, 15 to jest ilość rekordów wyświetlanych na danej stronie, możliwość sortowania po wartości SortBy -> można wpisać Id, w zmiennej SOrtDirection -> 1 to rosnąco, 2 -> malejąco. Można szukać po SearchWord -> czyli po numerze Id. Jeśli nie podasz wartości PageSize i PageNumber, wyświetlą się odpowiednie komunikaty. Jak pojawią się rekordy to na dole informacje dodatkowe: TotalPages -> ilość stron do przeglądania, itemFrom: pierwzy rekord, itemTo ostatni rekord na danej stronie wyswietlany, TotalItemsCount: znalezione wszystkie rekordy.

4. GET api/price -> wyświetla wszystkie wyszukane prices.
5. GET api/product -> wyświetla wszystkie wyszukane produkty.
Też taka sama metoda wyszukiwania jak w api/inventory.


 

