namespace Lab4

open Xamarin.Forms
open Xamarin.Forms.Xaml
open FunctionsUI

type StartPage() as this =
    inherit ContentPage()


    let actionButton () =
        let value = titleLabel.Text
        let chara = value.[11].ToString() |> int
        this.Navigation.PushAsync(MainPage(chara), true) |> ignore


    let initialize = base.Content <- setupRootView (actionButton)
