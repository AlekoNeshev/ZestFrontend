# Zest Frontend
## Твоят път към науката



Клиентското приложение на Zest използва технологията за cross-platform native applications .Net MAUI. Технологията е избрана зареди нейното таргетиране на платформите Windows, Android, Tyzen и IOS. Трябва да се има впредвид, че технологията е сравнително нова и все още се развива. Приложението е тествано на Windows и Android. Използва се Shell application. Приложение следва MVVM модел.


[Documentation for Zest](https://docs.google.com/document/d/1_bVuJu_zScgK3iwTjZTo0jCmbtr0PDWtfIlekuLMpU8/edit?usp=sharing)

## Страници

- MainPage (ContentPage)
-- Начална страница на приложението. Предоставя функционалност за влизане в акаунт в приложението чрез външен доставчик за автентикация. 
- AccountPage (ContentPage)
-- Страница с детайли за текущия потребител. Предоставя функционалност за излизане на акаунта.
- AddCommunityPage (ContentPage)
-- Страница с функционалност за създаване на общност.
- AddPostPage (ContentPage)
-- Страница с функционалност за създаване на публикация към общност
- ChatPage (ContentPage)
-- Страница, която визуализира съобщенията на текущия потребител с негов приятел в мрежата.
- CommentDetailsPage (ContentPage)
-- Страница, която визуализира коментар и неговите отговори. Предоставя функционалност за харесване, изтриване и публикуване на отговор към съответен коментар.
- CommuntiesPage (ContentPage)
= Страницата зарежда общностите в мрежата. Подрежда ги по даден филтър - All, Trending и Followed. Предоставя възможност за търсене на общност.
- CommunityDetailsPage (ContentPage)
-- Страницата визуализира детайлите на дадена общност, включително публикациите към нея, моито могат да се филтрират по Latest и Trending.
- CommunityModeratorsPage (ContentPage)
-- Страницата зарежда модераторите на дадена общност. Предоставя функционалност за одобряване или отхвърляне на кандидат-модератори (Функционалността е достъпна само за текущите модератори). Предоставя възможност за кандидатстване за модератор или премахване на текущия потребител като такъв.
- FriendsPage (ContentPage)
-- Страницата зарежда приятелите на текущия потребител. Предоставя възможност за търсене сред тях.
- PostDetailsPage (ContentPage)
-- Страницата визуализира дадена публикация и коментарите към нея. Предоставя функционалност за харесване и изтриване на публикацията или на някой от коментарите към нея.
- PostsPage (ContentPage)
-- Страницата зарежда публикации по даден филтър - Latest, Trending и Followed
- UserDetailsPage (ContentPage)
-- Страницата показва детайли за чуждите потребители в мрежата. Дава опция за последване на дадения потребител и зарежда негови следвани общности
- UsersPage (ContentPage)
-- Страницата зарежда другите потребители в мрежата. Предоставя възможност за търсене сред тях.

BindingContext на страниците сочи към съответния ViewModel.
## Навигация
- За телефон, приложението се възползва от Tabs. За Desktop е създадено специално NavigationView (ContentView) с цел удобство за потребителите.

## SignalR
- Клиентско приложение се възползва от предоставяната от API услуга за функционалност в реално време. Добавяне и премахване от групи обикновено става при навигация между страниците и зареждане на дадено съдържание.
