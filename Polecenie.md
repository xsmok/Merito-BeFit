# Mapowanie obiektowo-relacyjne: modele

## Cel lekcji

W ramach tej lekcji:

    wykorzystasz Entity Framework do zarządzania danymi w bazie danych,
    nauczysz się tworzyć relacje jeden-do-wielu w Entity Framework,
    będziesz operować na modelach zagnieżdżonych.

Więcej informacji na temat relacji między modelami można znaleźć pod poniższymi linkami:

    https://www.entityframeworktutorial.net/code-first/configure-one-to-one-relationship-in-code-first.aspx
    https://www.entityframeworktutorial.net/code-first/configure-one-to-many-relationship-in-code-first.aspx
    https://www.entityframeworktutorial.net/code-first/configure-many-to-many-relationship-in-code-first.aspx

## Opis modeli w aplikacji

Aplikacja Home Finances zawiera dwa modele: Category i Entry. Model Entry reprezentuje wpisy związane z wydatkami i wpływami w domowym budżecie, a kategorie pozwalają porządkować je dla lepszego rozeznania.

Każdy wpis jest obowiązkowo przypisany do dokładnie jednej kategorii, ale kategoria może mieć wiele wpisów. Jest to więc bez wątpienia relacja jeden-do-wielu.

## Model Entry

Poniżej znajduje się kod definiujący model Entry.

namespace HomeFinances.Models
{
    public class Entry
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public float Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateOnly Date { get; set; }
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        public virtual Category? Category { get; set; }
        [Display(Name = "Created by")]
        public string CreatedById { get; set; }
        [Display(Name = "Created by")]
        public virtual AppUser? CreatedBy { get; set; }
    }
}

Pierwsze zdefiniowane pole to Id. Jest to pole obowiązkowe przy wykorzystaniu EntityFramework. Jest ono typu int, więc zostanie ono automatycznie potraktowane jako pole liczbowe z AUTO INCREMENT. Kolejne interesujące pola to Name, Amount, Date. Wszystkie one reprezentują różne typy danych - tekstowy, liczbowy i data. Należy zwrócić uwagę, że pola wykorzystywane w EntityFramework są publiczne. Możemy jednak przedefiniować domyślny getter i setter, jeśli uważamy to za konieczne.

Gdzie cała magia? Podejście Code-First i wykorzystanie ORM pozwalają nam na przekształcenie takiego modelu w tabelę w bazie danych. Wykorzystywany jest do tego system migracji.

## Migracje

Polecenia Entity Framework można wykonywać na dwa sposoby: za pomocą konsoli menedżera pakietów lub zwykłego terminala. Poniżej przedstawiono instrukcję dla pierwszej metody. Korzystanie z terminala jest analogiczne, choć polecenia różnią się składnią.

Aby otworzyć konsolę menedżera pakietów w Visual Studio 2022, należy przejść do:

    Narzędzia > Menedżer pakietów NuGet > Konsola menedżera pakietów
    Tools > NuGet Package Manager > Package Manager Console

Po otwarciu konsoli można wprowadzić polecenie:

    Add-Migration <nazwa>

Nazwa migracji powinna być zgodna z zasadami nazywania klas w języku C#. Polecenie to generuje klasę migracji, która zawiera instrukcje przekształcane na kod SQL zgodny z wybraną bazą danych. System migracji analizuje klasy zarejestrowane w kontekście bazy i zapisuje zmiany. Jeśli klasa nie istniała wcześniej, migracja wygeneruje instrukcję tworzącą tabelę. W przypadku zmian w klasie (np. zmiana nazwy pola, typu danych, dodanie lub usunięcie właściwości), migracja zawiera odpowiednie polecenia modyfikujące tabelę. Gdy klasa zostanie usunięta, migracja wygeneruje instrukcję usunięcia tabeli.

Wprowadzenie zmian w klasie lub wygenerowanie migracji nie modyfikuje jeszcze bazy danych. Aby zastosować migracje, należy wykonać polecenie:

    Update-Database

To polecenie stosuje wszystkie migracje, które nie zostały jeszcze zaimplementowane w bazie. Dzięki sekwencyjnemu wykonywaniu migracji możliwe jest łatwe odtworzenie schematu bazy danych na innym serwerze. Należy jednak zachować ostrożność, ponieważ migracje nie są łatwe do cofnięcia. Wycofanie zmian wymagałoby ręcznej ingerencji, co może prowadzić do utraty danych.

## Podejście Database-First

Alternatywą do systemu migracji jest podejście Database-First. W tej konwencji nie korzystamy w ogóle z migracji, ale całość bazy danych tworzymy samodzielnie. O ile zachowamy właściwe konwencje nazewnictwa tabel lub wprost te nazwy podamy w kodzie, Entity Framework wciąż będzie wykorzystywać klasy model do zarządzania danymi, ale nie będzie ingerować w strukturę bazy danych.

To podejście jest mniej popularne, ponieważ w większości przypadków tabele generowane przez migracje są wystarczająco dobre. Dopiero w przypadku specyficznych struktur podejście Code-First może okazać się zawodne. Oczywiście, każdy deweloper może mieć własne preferencje.

## Model Entry

Poniżej znajduje się kod definiujący model Entry.

namespace HomeFinances.Models
{
    public class Entry
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public float Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateOnly Date { get; set; }
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        public virtual Category? Category { get; set; }
        [Display(Name = "Created by")]
        public string CreatedById { get; set; }
        [Display(Name = "Created by")]
        public virtual AppUser? CreatedBy { get; set; }
    }
}

W całym poprzednim opisie pominęliśmy masę kodu. Przy deklaracjach poszczególnych pól znajdują się dodatkowe linijki, które zaraz omówimy. Ponadto są jeszcze nieomówione pola, które zostają opisane w dalszych blokach.

Czym są te tajemnicze instrukcje w nawiasach kwadratowych przed deklaracją pola? To są tzw. atrybuty. Jak wiadomo, model danych, w tym jego odpowiednik z bazy danych, to nie tylko prosta deklaracja typu, ale często i wiele dodatkowych informacji, mniej lub bardziej koniecznych. Informacje te przekazywać możemy właśnie w atrybutach.

Atrybut MaxLength to przykład atrybutu walidacyjnego. Określa on wymagania co do zawartości pola. W tym przypadku maksymalną długość stringa. Można podejmować dyskusję, czy walidacja jest procesem właściwym dla modelu czy dla kontrolera. Faktem jednak pozostaje, że część walidatorów daje się łatwo przerobić na odpowiednią strukturę relacyjnej bazy danych. A skoro się da, to się nawet powinno dla zapewnienia integralności danych. Dlatego też niektóre atrybuty wykorzystywane są również w migracji.

Atrybuty Display i DisplayFormat odpowiedzialne są za wyświetlanie. Nie są one odwzorowywane w bazie danych, ale są wykorzystywane w widokach. W ten sposób wyświetlana etykieta (Display) czy format wyświetlania mogą zostać zdeklarowane w jednym miejscu i używane jednolicie we wszystkich widokach.

Atrybutów jest znacznie więcej i warto je poznać, ponieważ znacząco upraszczają sam kod. Można nawet zdefiniować własne. Większość z nich odpowiada za walidację danych lub ich wyświetlanie, ale niektóre mają specjalne funkcjonalności, np. podają informację przydatne jedynie dla migracji (np. NotMapped).

## Model Entry

Poniżej znajduje się kod definiujący model Entry.

namespace HomeFinances.Models
{
    public class Entry
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public float Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateOnly Date { get; set; }
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        public virtual Category? Category { get; set; }
        [Display(Name = "Created by")]
        public string CreatedById { get; set; }
        [Display(Name = "Created by")]
        public virtual AppUser? CreatedBy { get; set; }
    }
}

Omówimy teraz w jaki sposób można tworzyć relacje jeden-do-wielu na poziomie samego kodu C#.

Interesują nas dwa następujące po sobie pola: CategoryId oraz Category. Zastosowane podejście to jedna z możliwych konwencji, które można tutaj zastosować. Konwencja ta polega na tym, że jawnie wskazujemy nazwę pola, gdzie ma być trzymany klucz modelu w relacji (CategoryId), jednocześnie zostawiając sobie atrybut, w którym docelowo będziemy trzymać całość modelu.

Przy tej konwencji istotne jest, by nazwy pól tworzyły parę <nazwa>Id, <nazwa>. Inaczej zostaną potraktowane jako dwa osobne, niezależne pola. Jeżeli z jakiegoś powodu musimy tak zrobić, należy użyć atrybutu wskazującego, że jedno pole jest kluczem dla drugiego.

Analogiczny zabieg zastosowany jest dla powiązania z użytkownikiem. To powiązanie zostanie opisane w innej lekcji.

## Podsumowanie

Omówiliśmy szczegółowo model Entry wraz z procesem migracji. Przeanalizuj teraz kod definiujący model Category. Korzystając ze zdobytej wiedzy spróbuj zrozumieć, za co odpowiadają poszczególne fragmenty kodu i odpowiedz na kilka pytań.

Po pytaniach omówimy jeszcze konfigurację Entity Framework do współpracy z ASP.NET MVC.


## Konfiguracja Entity Framework

Aby nasza aplikacja ASP.NET MVC wykorzystywała Entity Framework potrzebujemy aby:

    Wszystkie konieczne biblioteki były zainstalowane.
    W pliku Program.cs było ustawione właściwe połączenie z bazą danych.
    Była stworzona klasa kontekstu bazy danych a w niej ustawione właściwe klasy modeli.

Dobór bibliotek zależy od wielu czynników. W podstawowej wersji wystarczy Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Tools oraz Microsoft.EntityFrameworkCore.<typ sql>. Pierwsza dostarcza stosownych narzędzi do wykonywania operacji. Druga dostarcza nam polecenia, których możemy użyć w konsoli, a trzecia implementuje funkcjonalności zależnie od wybranego typu bazy danych.

Poniżej znajduje się fragment Program.cs nawiązujący połączenie z bazą danych:
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

Należy zwrócić uwagę na dwie rzeczy: connectionString i UseSqlite.

Zmienna connectionString zawiera string z parametrami konfigurującymi połączenie z bazą danych. Ten string jest inny w zależności od rodzaju wykorzystywanej bazy danych oraz różnych parametrów. Aplikacja pobiera go z pliku appsetting.json.

Metoda UseSqlite odpowiada za to, że Entity Framework będzie wykorzystywał bazę dany SQLite i to z nią się połączy. By móc użyć tej metody konieczna jest biblioteka Microsoft.EntityFrameworkCore.Sqlite.

Ostatnim etapem konfiguracji jest kontekst bazy danych. W przykładowym projekcie jest to plik Data/ApplicationDbContext.cs. 
using HomeFinances.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
 
namespace HomeFinances.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<HomeFinances.Models.Category> Category { get; set; } = default!;
        public DbSet<HomeFinances.Models.Entry> Entry { get; set; } = default!;
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
 
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole() { Name = "Adult", NormalizedName = "ADULT" });
 
 
        }
    }
}

Istotne są dla nas w tej chwili dwie linie (14, 15). Definiują one atrybuty / własności klasy ApplicationDbContext. Typy tych atrybutów bazują na wbudowanej klasie generycznej DbSet. Te dwie linie rejestrują model w kontekście bazy danych, przez co klasa modelu będzie wykorzystywana w Entity Framework a ponadto definiują nazwę, po której będzie można się odwoływać do zasobów bazy danych.

## Ćwiczenie

To ćwiczenie stanowi wstęp do bardziej złożonego ćwiczenia - małej aplikacji webowej BeFit.

Na podstawie przykładowego projektu oraz zdobytej wiedzy, stwórz nowy projekt i nazwij go BeFit. W tym celu wykorzystaj szablon ASP.NET Core MVC. UWAGA! Jako typ uwierzytelnienia wybierz "Pojedyncze konta"!

Tak utworzony szablon aplikacji zawiera już gotowy kontekst bazy danych i podstawowy system użytkowników. Korzystając z przykładowego kodu z HomeFinances, ustaw w Program.cs, aby projekt łączył się z bazą danych SQLite. Należy w tym celu zmodyfikować pliki Program.cs i appsettings.json. Należy również usunąć folder Migrations (prawdopodobnie znajduje się w folderze Data).

Następnie zdefiniuj trzy modele. Jeden model ma opisywać typy ćwiczeń jakie można wykonywać na siłowni. Jedynym parametrem tego modelu jest jego nazwa (i oczywiście Id). Ustaw wybrane przez siebie ograniczenie długości nazwy.

Drugi model zawiera informację o sesji treningowej użytkownika. Chwilowo nie będziemy go łączyć z użytkownikiem, ale miejmy to z tyłu głowy. Dwie ważne informacje, które zawiera ten model, to data i czas rozpoczęcia treningu oraz data i czas zakończenia treningu. Jeśli potrafisz możesz spróbować zdefiniować w modelu walidator weryfikujący, czy data rozpoczęcia nie jest późniejsza niż zakończenia. Nie jest to jednak obowiązkowe, bo wymaga własnej definicji atrybutu.

Trzeci model łączy powyższe dwa. Model ten informuje, jaki typ ćwiczenia został wykonany w jakiej sesji treningowej przez jakiego użytkownika (to ostatnie chwilowo pomiń). Ponadto umieść w nim informacje o zastosowanym obciążeniu, liczbie serii i liczbie powtórzeń w serii.

Te trzy modele zarejestruj w kontekście bazy danych i przeprowadź migrację (stwórz i wykonaj). Przy pomocy oprogramowania do analizy plików baz danych sqlite, podejrzyj i przeanalizuj stworzoną strukturę bazy.

# Mapowanie obiektowo-relacyjne: polecenia

## Z modeli do kontrolerów i widoków

Główną zaletą wykorzystania ASP.NET Core MVC jest możliwość użycia szablonów. Na podstawie stworzonych przez nas modeli można łatwo wygenerować podstawowe kontrolery i widoki realizujące podstawowe operacje CRUD.

Aby to zrobić, należy:

    Kliknąć prawym przyciskiem myszy na folder z kontrolerami (w eksploratorze rozwiązań)
    Wybrać Dodaj > Nowy element szkieletowy (Add > New scaffolded item).
    Z dostępnej listy wybrać Kontroler MVC z widokami korzystający z programu Entity Framework (MVC Controller with views, using Entity Framework).
    W oknie, które się pojawi, wybrać nasz aktualny kontekst bazy danych (jeśli z jakiegoś powodu nie mamy żadnego, plusem można dodać nowy).
    Jeżeli sobie życzymy, możemy zmienić nazwę generowanej klasy. Domyślnie jest to nazwa modelu odmieniona w języku angielskim do liczby mnogiej (nawet gdy słowo nie jest w języku angielskim).

Po zatwierdzeniu stworzony zostanie kontroler realizujący podstawowe operacje CRUD, który jest doskonałą podstawą do dalszych modyfikacji. Ponadto mamy wytworzone podstawowe widoki z formularzami, które również stanowią podstawę do modyfikacji.

W przykładowym projekcie wszystkie one zostały zmodyfikowane. Dobrym ćwiczeniem jest stworzenie pustego modelu, skopiowanie do niego deklaracji modeli, wygenerowanie szablonowych kontrolerów i widoków i porównanie ich ze sobą.

## Wybieranie obiektów przy pomocy Entity Framework

Wygenerowane kontrolery dostarczają nam przykładów do podstawowych poleceń w Entity Framework. Są one mocno zbliżone do metod, które zawiera biblioteka Linq.

Spójrzmy na CategoriesController.cs. Znajdziemy tam następujące instrukcje wybierające dane z bazy:
 await _context.Category.Include(c => c.Entries).Select(c => new CategoryDTO(c)).ToListAsync();

To polecenie znajdziemy w akcji Index. Jego celem jest wybranie wszystkich kategorii, jakie mamy w bazie danych. Omówmy kolejne fragmenty.

Słowo await użyte jest, ponieważ polecenie jest asynchroniczne, ale nie możemy kontynuować działania, póki nie pobierzemy wszystkich danych.

Zmienna _context przechowuje kontekst bazy danych, dzięki któremu możliwe jest zarządzanie danymi. Obiekt ten ma atrybuty, które dają nam dostęp do danych. Nazwę atrybutu ustalamy sami przy rejestracji modelu w kontekście bazy danych. Tutaj, intuicyjnie, nazwa jest identyczna jak nazwa modelu.

Na samym końcu znajduje się .ToListAsync, które wykonuje zapytanie do bazy i przetwarza otrzymaną odpowiedź na listę. Zasadniczo poniższa skrócona wersja również zwróci wszystkie kategorie:
 await _context.Category.ToListAsync();

Co więc robią te dodatkowe polecenia? Czy są w ogóle potrzebne? Każda kolejna metoda dołączona do zapytania modyfikuje zapytanie do bazy danych oraz formę tego, co zostanie nam zwrócone. W tej uproszczonej wersji dostaniemy listę instancji klasy Category, ale pole Entries będzie puste.

Jeżeli dodamy polecenie .Include(c => c.Entries), wtedy w zapytaniu SQL zostanie zastosowany odpowiedni JOIN, który wybierze nam nie tylko samą kategorię, ale również podlegające jej wpisy. Nie dzieje się to automatycznie, ponieważ Entity Framework stosuje lazy loading, czyli nie pobiera modeli skojarzonych bez wyraźnego polecenia. (Przeciwieństwem lazy loading jest eager loading).

Dołączając więc to polecenie (koniecznie przed ToListAsync!), uzyskujemy więcej informacji. Zauważmy jednak, że wpisy również mają swoje modele skojarzone (w tym kategorię) - nie zostaną one pobrane w całości, a jedynie ich id dzięki zastosowanej wcześniej konwencji tworzenia klucza obcego.

Drugie polecenie, .Select(c => new CategoryDTO(c)), ma inne zadanie. Czysty model Category nie jest wystarczający dla naszych zastosowań. Dlatego automatycznie przekształcamy zwracane dane na instancję klasy CategoryDTO, o czym opowiemy później. W efekcie wynikiem końcowym nie jest kolekcja kategorii, ale kolekcja instancji CategoryDTO.

Zauważmy, że obie te metody jako argument przyjmują funkcję w notacji strzałkowej. Metoda Include musi mieć funkcję podającą pole, które ma zostać uzupełnione. Metoda Select natomiast potrzebuje funkcji przekształcającej obiekty.

Jest wiele innych ważnych poleceń, które można tutaj dodać. Odpowiadają one typowym klauzulom znanym z SQL. Np. metoda Where, która za argument przyjmuje funkcję zwracającą bool na podstawie parametrów kategorii i na podstawie tej funkcji filtruje zapytanie, czy metoda Order, służąca do sortowania.
 var category = await _context.Category
     .Include(c => c.Entries)
     .ThenInclude(e => e.CreatedBy)
     .FirstOrDefaultAsync(m => m.Id == id);

Kolejny fragment kodu pochodzi z akcji Details. Konstrukcja jest bardzo podobna, ale ma inny cel. Wcześniej chcieliśmy wybrać całą kolekcję kategorii, tutaj chcemy wybrać jedną. Polecenie .FirstOrDefaultAsync działa jak połączenie WHERE i LIMIT 1. Przekazujemy jej warunek, który musi zostać spełniony i wybieramy pierwszy element z zapytania, który zostanie zwrócony. Jeśli nie zostanie znalezione nic, zwróci nam null.

Wartym uwagi jest również polecenie .ThenInclude(e => e.CreatedBy), którego cel działania jest taki sam jak Include, ale działa nie na głównym modelu, ale na modelu skojarzonym. W ten sposób uzyskamy JOIN do kolejnej tablicy, łącząc się pośrednio przez model Entry. W efekcie uzyskana kategoria nie tylko będzie miała wszystkie swoje wpisy w polu Entries, ale same wpisy będą miały uzupełnione pole CreatedBy, które normalnie byłoby puste.
 var category = await _context.Category.FindAsync(id);

To ostatnie polecenie do omówienia. Znajduje się ono w akcji Edit dla zapytań POST. Jego celem jest znalezienie kategorii o podanym id. Różnica między tym a poprzednim jest techniczna. Metoda FindAsync pozwala na użycie tylko jednego warunku - klucza własnego. Poprzednio omawiane FirstOrDefaultAsync działa na dowolnym zapytaniu. Wynik obu jest ten sam - obiekt lub null.

## DTO

To czym jest to CategoryDTO czy EntryDTO i po co nam? Język C# jest silnie typowanym językiem obiektowym. Zatem, jeśli chcemy utworzyć obiekt klasy Category lub Entry, musimy podać mu wszystkie dane, jakie są w modelu zdeklarowane, o ile nie są nullowalne. Dopuszczenie jednak nullowalności danego pola sprawia, że jest ono nullowalne w bazie danych. A nie zawsze tego chcemy.

Doskonałym przykładem są pola dla modeli podrzędnych, np. Entries dla kategorii czy Category dla wpisów. Ze względu na lazy loading, nie są one domyślnie uzupełniane, co może prowadzić do różnego rodzaju błędów. Dlatego właśnie pole Category jest nullowalne (co nie wpływa na bazę danych!) a pole Entries ma wartość domyślną.

Możemy mieć jednak pola, których wartości będą uzupełniane w innym momencie czy w inny sposób. Przykładem są CreatedById i CreatedBy w modelu Entry. Są one uzupełniane automatycznie przy tworzeniu, a nie podawane przez użytkownika. Temu zagadnieniu poświęcona jest oddzielna lekcja.

Innym przykładem jest pole Balance w CategoryDTO. Jest to nowe, nieistniejące w oryginalnym modelu pole. Jego celem jest wyliczanie salda danej kategorii. Saldo wyliczane jest automatycznie na podstawie wpisów. Zdeklarowanie więc go w zwykłym modelu, o ile nie jest robione umiejętnie, może sprawić, że pojawi się całkowicie zbędna kolumna w tabeli w bazie danych. Dlatego dla lepszej separacji oddzielono model od jego reprezentacji, czyli DTO.

DTO = Data Transfer Object. To rodzaj klasy wyróżniany ze względu na przeznaczenie. Służy on, zwłaszcza w obiektowych językach silnie typowanych, do tworzenia różnego rodzaju reprezentacji modeli. Dzięki temu możemy stworzyć obiekt o mniejszej liczbie pól niż model (np. przy tworzeniu obiektu, który sam sobie uzupełni niektóre wartości) lub wręcz przeciwnie, obiekt o większej liczbie pól, gdzie te dodatkowe pola nie będą przechowywane w bazie danych.

## Agregacje

Wybierając obiekty w SQL poleceniem SELECT możemy użyć funkcji agregujących (zliczania, sumowania, średniej, maksimum, minimum itd.) i/lub polecenia GROUP BY.

Oczywiście Entity Framework zapewnia stosowne możliwości do użycia tych funkcjonalności. Działają one analogicznie do podanych wcześniej - jako argument przyjmują funkcję, która wskaże pole, po którym chcemy sumować, zliczać lub pole, po którym chcemy grupować.

Ich działanie jednak jest odrobinę mniej intuicyjne niż poprzednich i różni się w zależności od konkretnego przypadku. Dlaczego? Kolejność może mieć znaczenie. Całe szczęście sztuczna inteligencja radzi sobie z tym bardzo dobrze.

## Tworzenie, edycja, usuwanie

Jak stworzyć obiekt, a dokładniej mówiąc, jak stworzyć obiekt i umieścić go w bazie danych?

Tworzymy sobie obiekt normalnie jak w programowaniu obiektowym (byle był to obiekt klasy zarejestrowanej jako model) i przekazujemy go do kontekstu bazy danych.

Przykład:
        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adult")]
        public async Task<IActionResult> Create([Bind("Id,Name,Color")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

To metoda odpowiedzialna za przyjęcie formularza wysłanego metodą POST. Przyjmuje ona parametry Id, Name, Color z formularza i na ich podstawie tworzy obiekt klasy Category (zauważmy, że Entries są domyślnie puste). Jako że są to wszystkie pola, które są konieczne do stworzenia obiektu, pod zmienną category dostępny jest nowy obiekt.

(Zasadniczo pole Id nie jest przekazywane, ale jest generowane automatycznie. To taki wyjątek.)

Następnie sprawdzamy, czy walidacja się powiodła - jeśli nie (np. któreś pole nie zostało przesłane lub miało błędy format) wracamy do formularza tworzenia (i przekazujemy mu już przesłane dane oraz informacje o błędach). Jeśli walidacja się powiodła, zapisujemy model.

Entity Framework do modyfikacji danych w bazie wymaga dwóch kroków - zaktualizowania ich w kontekście i zapisania zamian. W tym przypadku metoda Add odpowiada za dodanie nowego obiektu do kontekstu. Dopóki nie zostanie wywołane polecenie zapisujące, zmiany nie zostaną odwzorowane w bazie danych. Ma to na celu ograniczenie liczby zapytań. Np. jeśli chcemy dodać wiele modeli, to tworzymy wiele obiektów, każdy dodajemy do kontekstu a polecenie zapisujące wywołujemy na końcu. Dzięki temu liczba poleceń INSERT zostanie ograniczona do minimum.

Edycja działa w sposób analogiczny. Pobieramy model z bazy, modyfikujemy pola i przekazujemy zaktualizowany obiekt do kontekstu. Alternatywnie możemy nawet nie pobierać niczego z bazy, a jedynie stworzyć obiekt o tym samym id co już istniejące. Przekazanie następuje metodą Update i tak samo należy potem zapisać zmiany.

Usuwanie obiektu wymaga wywołania polecenia Remove i zapisania zmian.

## Ćwiczenie

To ćwiczenie stanowi kontynuację ćwiczenia z poprzedniej lekcji. Otwieramy więc utworzony już projekt BeFit, w którym mamy trzy stworzone modele wraz z kontekstem bazy danych. Co należy zrobić?

W pierwszej kolejności przy pomocy elementów szkieletowych tworzymy kontrolery i widoki dla podstawowych operacji CRUD. W ten sposób możemy łatwo dodać sobie przykładowe treści do testów. Pamiętaj, że jeszcze nie mamy tutaj systemu użytkowników (znaczy, mamy, ale nic jeszcze nie robi)!

Na co zwracamy uwagę? W formularzach tworzenia i edycji dla modelu opisującego wykonane ćwiczenia wraz z parametrami są listy rozwijane. O ile mamy jakieś typy ćwiczeń i sesje ćwiczeniowe, listy te nie są puste, ale są niefunkcjonalne - wyświetlają id zamiast czegoś czytelnego. Należy to zmienić. W wygenerowanym kontrolerze są cztery miejsca w których tworzone są dwa obiekty SelectList. Znajdź je i zmodyfikuj. Możesz posłużyć się dokumentacją, by wiedzieć za co odpowiadają parametry lub spojrzeć na przykładowy projekt.

Następnie sprawdź, czy na pewno w modelach wszystkie pola mają stosownie ustawiony atrybut [Display]. Pola muszą mieć przynajmniej nazwę, a najlepiej i opis. Jeśli są dobrze ustawione, będzie to widoczne w formularzach.

Ostatnim, najtrudniejszym, jest stworzenie własnego kontrolera. Tworzymy sobie pusty kontroler StatsController (czyli nie generujemy go jako element szkieletowy). Dostosowujemy go, by miał dostęp do kontekstu bazy danych. Następnie tworzymy mu akcję Index (GET), która wyświetli statystyki wykonanych ćwiczeń. Każdy typ ćwiczeń ma mieć wyświetlone, ile razy w ciągu ostatnich czterech tygodni było dane ćwiczenie wykonywane, ile łącznie powtórzeń zostało wykonanych (liczba serii * liczba powtórzeń serii, zsumować po wszystkich sesjach) oraz jakie było średnie i maksymalne obciążenie.

By wyświetlić dane stwórz stosowny widok, który wygeneruje odpowiednią tabelkę.

Jeśli taka jest Twoja preferencja, stwórz sobie model do przechowywania statystyk. Ważne jest to, by nie rejestrować go w bazie danych - model musi być generowany dynamicznie, na żywo, przez Entity Framework.

W kolejnych lekcjach dostosujemy ten projekt do systemu użytkowników.

# MS Identity - wstęp

## Wprowadzenie

Biblioteka Microsoft Identity to zestaw narzędzi programistycznych umożliwiających bezpieczne uwierzytelnianie użytkowników i autoryzację dostępu do zasobów w ekosystemie Microsoft. Zapewnia obsługę standardów takich jak OAuth 2.0 i OpenID Connect, umożliwiając aplikacjom integrację z usługami Microsoft Entra ID (dawniej Azure AD), Microsoft 365 oraz innymi systemami zgodnymi z tymi protokołami. Biblioteka ta pozwala na implementację różnych metod logowania, w tym uwierzytelniania użytkowników w organizacji, aplikacji wielodostępowych oraz scenariuszy z kontami osobistymi Microsoft.

## Konfiguracja projektu

Wykorzystując szablon ASP.NET Core MVC, można wybrać typ uwierzytelnienia, np. pojedyncze konta (standardowy sposób zarządzania użytkownikami). W ten sposób już na etapie tworzenia projektu system użytkowników zostanie włączony do kodu w wielu miejscach. Z tego też powodu warto uczynić to na samym początku. Stworzenie projektu bez autentykacji i późniejsze dołączenie komponentów biblioteki Microsoft Identity jest bardzo żmudne.

## Modele użytkownika

Biblioteka Microsoft Identity zapewnia wbudowaną klasę IdentityUser, która zawiera szereg pól koniecznych do poprawnego i bezpiecznego działania systemu autentykacji. Są tam już m.in: nazwa użytkownika, adres e-mail, pole na hash hasła itd.

Zaleca się jednak stworzenie własnego modelu użytkownika poprzez dziedziczenie. Nawet jeżeli w zupełności satysfakcjonuje nas oferta IdentityUser, warto zrobić puste dziedziczenie i stworzyć własną klasę. W przyszłości bowiem, gdy pojawi się potrzeba dodania do modelu użytkownika dodatkowych pól (np. imię i nazwisko), zmiana klasy nie będzie konieczna i tym samym będzie mniejsze prawdopodobieństwo zepsucia integralności projektu.

W pliku Program.cs znajduje się konfiguracja usługi autentykacji. Metoda AddDefaultIdentity wskazuje klasę użytkownika. Należy więc tam podać naszą nową klasę użytkownika.

Kolejnym miejscem, w którym należy podać właściwą klasę użytkownika jest DbContext.cs. Klasa kontekstu bazy danych dziedziczy po IdentityDbContext, który jest zależny od klasy użytkownika.

Ostatnim miejscem do weryfikacji jest częściowy widok _LoginPartial.cshtml. Tam na początku przywoływane są usługi zarządzające logowaniem i użytkownikami. Należy im również wskazać właściwą nazwę klasy użytkownika.

Pomieszanie naszej własnej klasy użytkownika z IdentityUser może doprowadzić do błędów kompilacji lub błędów w działaniu. Należy więc uważnie przeanalizować całość projektu i konsekwentnie wszystkie miejsca zmienić. Dlatego tak istotne jest zrobienie tego od samego początku.

## Role użytkowników

W odróżnieniu od wielu frameworków, Microsoft Identity nie dostarcza gotowych ról. Domyślnie odróżnia się użytkownika zalogowanego i nie (gościa / użytkownika anonimowego). Mamy jednak zaimplementowany mechanizm używania ról, tylko trzeba je stworzyć.

Opis sposobu tworzenia ról i zarządzania nimi w przykładowym projekcie umieszczony jest w osobnej lekcji. System ten jest o tyle elastyczny, że trudno go opisać spójnie. W podstawowych zastosowaniach wydaje się wymagać więcej pracy niż przynosi efektów, ale został stworzony pod bardziej złożone projekty, w których ról jest bardzo dużo.

# Wiązanie modeli z użytkownikami

## Cel lekcji

W ramach tej lekcji:

    stworzysz klucz obcy z dowolnego modelu do modelu użytkownika,
    powiążesz model z aktualnie zalogowanym użytkownikiem.

## Model Entry

Poniżej znajduje się kod definiujący model Entry.

namespace HomeFinances.Models
{
    public class Entry
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public float Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateOnly Date { get; set; }
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        public virtual Category? Category { get; set; }
        [Display(Name = "Created by")]
        public string CreatedById { get; set; }
        [Display(Name = "Created by")]
        public virtual AppUser? CreatedBy { get; set; }
    }
}

Zwróćmy uwagę na relację CreatedBy - stworzona jest ona według tej samej konwencji co Category, czyli zawiera pole klucza obcego i pole do wstawienia modelu użytkownika. Zauważmy, że id użytkownika jest stringiem! Poza tym nie ma tutaj nic nowego.

## Pozyskiwanie danych o aktualnie zalogowanym użytkowniku

Aplikacje webowe w większości przypadków są spersonalizowane. Użytkownicy widzą tylko swoje dane i mogą tylko swoje dane tworzyć, edytować, usuwać itd. Aby móc to zrobić, musimy mieć sposób na łatwe pozyskanie danych o aktualnie zalogowanym użytkowniku.

Sposobem na pozyskanie tych danych jest wykorzystanie systemu Claims. Nie będziemy go opisywać w szczegółach, a skupimy się na najważniejszym przykładzie: pozyskiwaniu id.
private string GetUserId()
{
     return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
}

Powyższa metoda zwracać będzie id użytkownika, o ile jakiś jest zalogowany. W przeciwnym wypadku będzie to pusty string. Jak widać, nie jest to ani trochę intuicyjne jak na tak podstawową funkcjonalność. Znacznie prostsza metoda o nazwie analogicznej do powyższej została usunięta w ASP.NET Core 6. Należy o tym pamiętać, wyszukując informacje w internecie.

## Tworzenie modeli i przypisywanie ich do użytkownika

Domyślnie wygenerowany kontroler i widoki pozwalają wybrać użytkownika z listy, jak zwykły model. Nie ma to w większości przypadków logicznego sensu. Należy więc usunąć odpowiedzialne za to pola w formularzach i zmodyfikować odpowiednio kontroler. Poniżej przedstawiamy akcje Create,
        // GET: Entries/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }
 
        // POST: Entries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Amount,Date,CategoryId")] EntryDTO entryDTO)
        {
            Entry entry = new Entry()
            {
                Id = entryDTO.Id,
                Name = entryDTO.Name,
                Amount = entryDTO.Amount,
                Date = entryDTO.Date,
                CategoryId = entryDTO.CategoryId,
                CreatedById = GetUserId()
            };
            
            if (ModelState.IsValid)
            {    
                _context.Add(entry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", entry.CategoryId);
            return View(entry);
        }

Zwróćmy uwagę na istotną różnicę w tej akcji a akcji domyślnie generowanej ze szkieletu. Zamiast używać tutaj modelu Entry, wykorzystujemy EntryDTO. Jak można zauważyć w samym kodzie, różnią się one tym, że EntryDTO nie posiada CreatedBy. Dzięki temu walidacja formularza, który nie przesyła id użytkownika, przejdzie pomyślnie.

Oczywiście, można byłoby dołączać id użytkownika jako ukryte pole w formularzu. Jednak jest to rozwiązanie niepoprawne. I tak należy koniecznie zweryfikować to id po stronie kontrolera, by użytkownik końcowy nie mógł tam wstawić id innego użytkownika.

Następnie, mając już przekazane dane do modelu DTO, możemy stworzyć prawdziwy model, przepisując dane i dodając id aktualnego użytkownika.

## Ćwiczenie

Kontynuujemy pracę z projektem BeFit. Dziś dodamy do niego użytkowników. Ze względu na to, że już wcześniej pracowaliśmy nad tym projektem i prawdopodobnie możemy mieć jakieś dane w bazie, zaleca się usunąć bazę danych i wszystkie migracje. Za chwilę dokonamy modyfikacji i wtedy stworzymy nową migrację.

W pierwszej kolejności stwórz model użytkownika. Nazwij go jak chcesz, byle dziedziczył po IdenitityUser (jak w poprzedniej lekcji) i użyj go konsekwentnie we wszystkich miejscach kodu.

Następnie powiąż model sesji treningowych i ćwiczeń wykonanych w tych sesjach z modelem użytkownika. Nazwij połączenie dowolnie. Stwórz migrację i zaktualizuj (stwórz) bazę danych.

Do kontrolerów dla sesji i wykonanych ćwiczeń dodaj metodę pobierającą id użytkownika (możesz skopiować z przykładowego projektu).

W akcjach Create w obu wymienionych kontrolerach zastosuj wiązanie modeli z aktualnie zalogowanym użytkownikiem. W tym celu musisz stworzyć odpowiednie modele DTO.

Możesz przetestować efekty swoich działań i sprawdzić je w bazie danych.

# Ograniczanie dostępu do podstron

## Ograniczanie dostępu osobom niezalogowanym

Sposobem na to by w łatwy sposób ograniczyć dostęp osobom niezalogowanym do danego kontrolera / akcji (czyli do konkretnych podstron) jest użycie atrybutu [Authorize]. Wstawiony przed deklaracją klasy kontrolera sprawia, że każda próba dostępu do akcji zawartych w samym kontrolerze będzie w pierwszej kolejności wywoływać sprawdzenie, czy użytkownik się zalogował. Jeśli nie, zostanie przekierowany na stronę logowania.

Atrybut ten możemy wykorzystać nie tylko by zabezpieczyć cały kontroler, ale również, by zabezpieczyć poszczególne akcje. Możemy przecież chcieć dać użytkownikowi dostęp do wyświetlania treści a nawet ich tworzenia bez posiadania konta, a jednocześnie nie chcieć by mógł jakieś treści edytować czy usuwać. Wstawiamy wtedy atrybut przed deklaracją metody odpowiedzialnej za daną akcję.

Bardzo ważne jest to, by pamiętać o zabezpieczeniu nie tylko akcji odpowiedzialnych za obsługę metody GET, ale również za zabezpieczenie metody POST. Ukrycie formularza nie powstrzyma nikogo przed zasymulowaniem jego przesłania.

## Filtrowanie względem użytkownika

Jak już wiemy, Entity Framework pozwala łatwo tworzyć zapytania SQL, w tym używać klauzuli WHERE. W ten sposób możemy wybrać tylko i wyłącznie modele, które są przypisane do danego użytkownika. Jest to bardzo częsty zabieg, zwłaszcza ze względów bezpieczeństwa i ochrony prywatności. Może być tak, że chcemy ograniczyć dostęp użytkownika jedynie do jego własnych modeli.

Aby to zrobić, należy uzyskać dane zalogowanego użytkownika (np. jego id) i dostosować działanie kodu. Zależnie od sposobu działania kontrolera można to zrobić na różne sposoby. Akcje, które w pierwszej kolejności wybierają dane (np. Index, Details) mają od samego początku zbudowane zapytanie, do którego wystarczy dodać .Where z filtrowaniem po odpowiednim polu. W ten sposób szablonowa akcja Index wybierze tylko te modele, które należą do użytkownika. Być może nie wybierze nic.

Akcja Details natomiast przyjmuje przede wszystkim id modelu. Jeżeli dodamy id użytkownika, to gdy ktoś spróbuje otrzymać dostęp do cudzego modelu, otrzyma błąd 404 Not Found. Jest to popularna praktyka, bo poza faktycznym zabezpieczeniem dostępu, nie przekazujemy informacji, że dany zasób w ogóle istnieje. Jeżeli chcemy poinformować, że zasób istnieje, ale użytkownik nie ma do niego dostępu, trzeba rozbić to na kroki. Najpierw pobieramy po id modelu, jeśli nie ma - 404. Następnie sprawdzamy id użytkownika, jeśli niezgodne - 403 Forbidden. Na koniec zwracamy sam widok, o ile wcześniej akcja nie została przerwana. Ta procedura jest przydatna przy bardziej złożonych systemach autoryzacyjnych, gdy nie można tego prosto zakodować w SQL.

Akcje Edit oraz Delete wygenerowane przez szablon działają w sposób specyficzny. Delete sprawdza jedynie, czy istnieje model o danym id. Można zmodyfikować zapytanie sprawdzające, dodając drugi warunek, czyli id użytkownika. Żeby ktoś nie usunął czyjegoś modelu... Akcja Edit dla metody POST jest bardziej kłopotliwa. Nie mamy w niej domyślnie pobierania samego modelu, a jedynie tworzenie nowego z gotowym id. Należy więc, podobnie jak w Delete, dodać sprawdzenie, czy model o danym id i przypisany do danego użytkownika faktycznie istnieje zanim spróbujemy dokonać aktualizacji.

Ponownie należy pamiętać, że takowe zabezpieczenia należy umieścić i w wersji dla GET i w wersji dla POST.

## O czym warto pamiętać

Wiele akcji w kontrolerach, zwłaszcza napisanych własnoręcznie, wykonuje zapytania do bazy danych. Każde z nich należy sprawdzić, czy przypadkiem nie powinno być przefiltrowane po użytkownikach.

Na przykład, gdy formularze tworzenia czy edycji mają listy wyboru, domyślnie te formularze są pobierają wszystkie modele z bazy. A często może być tak, że chcemy wybrać jedynie te, do których użytkownik ma dostęp.

Zapytania zbierające dane są szczególnie wrażliwe na pominięcie właściwego filtrowania. Jeżeli zwracają one jedynie zliczenie, sumę, średnią itp., należy zadbać, by liczyły je tylko na podstawie właściwych danych. Bardzo trudno jest wychwycić na etapie testów, że zliczamy zbyt wiele, bo przecież tego nie widać (chyba że znamy wyniki).

## Ćwiczenie

Kontynuujemy pracę nad projektem BeFit.

Gdy modele są już powiązane z użytkownikami, należy zabezpieczyć właściwie kontrolery. Interesuje nas kontroler sesji treningowych i kontroler wykonanych ćwiczeń. Oba z nich powinny być dostępne tylko dla zalogowanych użytkowników. Należy przejrzeć każdą akcję i każdą zabezpieczyć, by użytkownik miał dostęp jedynie do swoich zasobów.

Pamiętaj o zabezpieczeniu list wyboru.

Następnie należy podobne zabezpieczenia i filtry nałożyć na kontroler ze statystykami - użytkownik ma widzieć statystyki wyliczone na podstawie jego własnych danych.

Chwilowo zostaw kontroler z typami ćwiczeń niezabezpieczony. Tym zajmiemy się w kolejnej lekcji.

# Role użytkowników

## Podstawy działania systemu ról

W Microsoft Identity role można rozumieć jak pewne grupy, do których można przypisać użytkownika, nadając mu pewne uprawnienia. Uprawnienia te nie są formalnie wpisane w bazie, ale są one zakodowane w kontrolerach. Można w łatwy sposób ograniczyć dostęp do kontrolera tylko do wybranych ról.

Mamy również dostępny cały zasób funkcji, które sprawdzają przynależność użytkownika do roli, dodanie roli użytkownikowi lub jej pozbawienie. Dzięki temu możemy również zmieniać zasady działania kontrolera samego w sobie. Przykładowo można zrobić w ten sposób, że edycja jakiegoś zasobu jest dostępna dla użytkownika, który stworzył dany zasób lub administratora, nawet gdy administrator nie jest jego twórcą.

Role rozpoznawane są po nazwach, dlatego to od programisty zależy, jak się będą nazywać i co będą robić. Nie mamy żadnych wbudowanych, specjalnych ról.

Największą trudnością jest brak prostego mechanizmu tworzenia ról (seedowania) ani nadawania ich użytkownikom. Wszystko należy zaimplementować samemu, w zależności od potrzeb. W tej lekcji omówimy jak zostało to zrobione w przykładowym projekcie. Przykład nie jest ciekawy ani nawet użyteczny, ale pokazuje podstawowe opcje zarządzania rolami.

## Tworzenie ról

Tworzenie ról może odbywać się na dwa sposoby - można stworzyć managera ról podobnie jak zwykły kontroler. Jednakże nie ma to sensu, gdyż role muszą być z góry zakodowane, więc po co tworzyć je dynamicznie.

Tutoriale internetowe dostarczają różnych propozycji w tym temacie. Można spotkać dwa główne podejścia: wprowadzanie ról do bazy danych przy uruchomieniu oraz wprowadzanie ról do bazy danych przy migracji.

To drugie podejście zostało wykorzystanie w przykładowym projekcie. W kontekście bazy danych nadpisujemy metodę OnModelCreating, dodając do niej instrukcję HasData, która pozwala nam dodać domyślne dane, które zostaną wprowadzone do bazy danych w procesie migracji. Jest to skuteczny i czysty sposób, który nie spowoduje nieprzewidzianych konsekwencji.

## Dodawanie użytkowników do ról

Dodawnie użytkownika do roli odbywa się w RolesController. W pierwszej kolejności w konstruktorze nakazujemy przyjąć usługę UserManager, którą do tego wykorzystamy. Jest ona dostarczana automatycznie przez ASP.NET.

Kontroler ma tylko jedną akcję - ta akcja pobiera użytkowników z sesji i jeśli jest to jeden użytkownik, pobiera jego dane z bazy i przypisuje mu rolę dorosłego.

To rozwiązanie jest jednym z najgorszych możliwych pomysłów. Każdy może wejść pod odpowiedni link i dostać uprawnienia. Ale zarządzanie rolami nie jest najistotniejszym elementem tego projektu, więc rozwiązaliśmy to w ten sposób.

Prawidłowe rozwiązanie powinno tworzyć domyślnego użytkownika w odpowiedniej roli (pierwszego) a także stworzyć kontroler pozwalający nadawać i odbierać role innym użytkownikom.

## Zabezpieczanie rolami

Przykładowe zabezpieczenie przy pomocy ról ma miejsce w kontrolerze kategorii. Tam niektóre akcje są zabezpieczone, aby tylko dorośli mogli ich dokonywać.

Wykorzystujemy do tego atrybut [Authorize(Roles = "Adult")]. Zwróćmy uwagę, że role są podane w postaci stringa. Jeżeli chcemy użyć ich więcej, rozdzielamy je przecinkiem.

## Ćwiczenie

Kontynuujemy pracę nad projektem BeFit.

Zabezpiecz kontroler z typami ćwiczeń. Dostęp do wyświetlania typów ćwiczeń powinni mieć wszyscy, nawet niezalogowani użytkownicy.

Natomiast metody odpowiadające za tworzenie, modyfikację i usuwanie typów ćwiczeń należy zabezpieczyć, aby tylko administrator miał do tego dostęp.

Pamiętaj stworzyć mechanizm tworzenia roli administratora w bazie danych. Nie musisz implementować żadnego sposobu na nadawanie tej roli użytkownikom.