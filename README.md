[![Build Status](https://travis-ci.org/llepecki/Spammer.svg?branch=master)](https://travis-ci.org/llepecki/Spammer)
# Spammer
Manage your subscriptions the hard way.
## Dzień dobry
Na repozytorium umieściłem zadanie, wysłane mi przez panią Aleksandrę Blikiewicz. Teoretycznie, zadanie miało zająć dwa wieczory, ale albo ja chciałem za dużo w nim zrobić, albo jest jednak na trochę dłużej. Ja, niestety, nie miałem więcej czasu niże te 8 godzin i zrobiłem tyle ile zrobiłem, starając się dotknąć wszystkich istotnych zagadnień i pozostawić rozwiązanie w stanie działającym, ale do dalszego rozwoju.
## Co się udało, co się nie udało
- backend in .NET using C# - Projekt napisany w **.NET Core 2.0**, obsłużone zarządzanie subskrypcjami oraz ich wysyłanie.

- frontend in any choosen technology - **Angular** (najnowszy) + **Bootstrap**. Został stworzony frontend do zarządzania subskrypcjami, do zrobienia część dotycząca wysyłania maili (w tej chwili można to zrobić tylko przez API, np. **Postmanem**)

- SQL database to store persistent data - No niestety, tutaj się nie udało. Z braku czasu zaimplementowałem tylko proste repozytorium in-memory, ale pod jego interfejsem można łatwo zaimplementować bazę SQLową. Dodałem już **Dappera**, żeby stworzyć drugą implementację, ale na tym etapie muszę już sam sobie narzucić pewne ograniczenie. Gdybym jednak je implementował, to model danych wyglądałby następująco:

| int | nvarchar |
| ------------ | ------------ |
| id (klucz)  |  mail (unique)  |

| nchar(2) | nvarchar |
| ------------ | ------------ |
| abbreviation (klucz)  |  description  |

| int | nchar(2) |
| ------------ | ------------ |
| id (klucz z pierwszej tabelki)  |  abbreviation (klucz z drugiej tabelki)  |

Czyli jedna tabelka z adresami, druga z tematami i do tego tabelka asocjacyjna. To wszystko schwane za interfejsem `ISubscriptionRepository` i mapowaniem za pomocą **Dappera**.

- unit tests written - Dodałem testy do kluczowego kontrolera, ponieważ tam jest najwięcej ciekawej logiki działania systemu. System jest zaprojektowany tak, że klasy są luźno ze sobą powiązane (interfejsami) i tak naprawdę unit testy można napisać do większości rzeczy (jak ma się odpowiednio dużo czasu). Użyłem biblioteki **xUnit** a do mockowania **Moq**.

- SPA - Jest, oczywiście można na pewno sporo popracować nad warstwą wizualną (np. ładniejsze komunikaty w Toastr) i dodać walidację maila również po stronie UI (po stronie API jest). Znowu - projekt jest rozwojowy, szczególnie jeżeli chodzi o UX, ale robi to co ma robić.
## Rozwiązania techniczne
### Autoryzacja
Wydaje mi się, że jest ona istotną kwestią w tego typu aplikacji. Po pierwsze, tylko administrator powinien mieć możliwość pobrania pełnej listy subskrybentów, po drugie tylko on powinien być uprawniony do wysyłania wiadomości. Uwierzytelnianie, które dodałem, jest oczywiście banalne, a basic authentication nie nadaje się na żadną produkcję (choćby przez brak https), ale chciałem w ten sposób zwrócić uwagę, że jako taka, autoryzacja jest w tym projekcie istotna. Autoryzację i uwierzytelnianie można w każdym razie łatwo podmienić na bezpieczniejsze.
Przetestować można próbując pobrać listę wszystkich subskrybentów lub próbując wysłać do nich wiadomość. Bez nagłówka `Authorization` w requeście, API odpowie **401** (Unathorized). Po dodaniu uwierzytelniania typu basic z danymi `admin:drezyn4` (zahardkodowane w API) system pozwoli na wykonanie żądania.
### Kody HTTP
API odpowiada zgodnie z tym co robi. Np. `ChangeSubscription` (PUT) wysłaniem stworzonego lub zmodyfikowanego obiektu w body, a w headerze będzie znajdował się link do niego, kod **201** (Created). Jeżeli `ChangeSubscription` tak samo jak `Unsubscribe` (DELETE) spowoduje usunięcie adresu z bazy (pusta lista tematów) dostaniemy kod **204** (NoContent).
### Walidacja adresu e-mail
Zrobiona za pomocą własnego `RouteConstraint`. API nie wpuszcza nieprawidłowych adresów e-mail.
### Wysyłanie wiadomości
Wysyłanie wiadomości zaimplementowałem w ten sposób, że są one zapisywane do predefiniowanej lokacji (const w klasie `Startup`) w odpowiednim formacie (szczegóły w klasie `SendToDirectory`). Wysyłanie jest oczywiście schowane za interfejsem, przez co łatwo można podmienić implementację na taką, która naprawdę wysyła maile.
### CI
Projekt jest podpięty pod Travisa. W nagłówku tej wiadomości widać aktualny stan builda.
### Podsumowanie
Jeżeli będą mieli Państwo jakieś pytania, bądź będą chcieli Państwo przedyskutować stworzone przeze mnie rozwiązanie, zachęcam do kontaktu. System jest oczywiście bardzo toporny, ale biorąc pod uwagę reżim czasowy, jaki musiałem sobie narzucić, uważam, że działa całkiem sensownie i jest dobrą podstawą do dalszego rozwoju.
#### Łukasz Łepecki
