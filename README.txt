Projekt leírása:
A projekt egy bejelentkezési felületet, majd ezt követően egy statisztikai kimutatást végző formot valósít meg.
A bejelentkezéshez tartozó felhasználókat a Users lista tárolja, amelyhez jelenelg a:
User1 - Password1 (inaktív)
User2 - Password2 (aktív)
User3 - Password 2 (aktív)
Felhasználók és jelszavak vannak hozzárendelve. Háromszori rossz próbálkozás esetén a form lezár és 30 másodperc várakozási idő után engedi újra belépni a felhasználót. Csak aktív felhasználók számára engedélyezett a belépés.
A Unit Test az aktív felhasználók belépési engedélyére vonatkozik.

A belépés után a statisztika gombra kattintva egy datagridview segítségével tekinthetjük végig a koronavírus hazai alakulását. Az adatokat lementhetjük csv formátumban, valamint xml formátumú fájlokat is importálhatunk.
A grafikon gombra kattintva diagramon is képet kaphatunk az esetszámok alakulásáról.
Az Excel gomb pedig egy új excel munkafüzetet generál az adatokból, valamint két diagramot is megkapunk az esetszámokról és a halálozási adatokról.
Az R számítása fülön az aktuális esetszámok megadásával kiszámíthatjuk a járvány aktuális R mutatójának értékét.

A felhasznált adatok forrása: https://data.europa.eu/euodp/hu/data/dataset/covid-19-coronavirus-data
