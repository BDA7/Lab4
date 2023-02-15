namespace Lab4

open Xamarin.Forms

type App() =
    inherit Application(MainPage = NavigationPage (new StartPage ()))

