namespace Lab4

open Xamarin.Forms
open Xamarin.Forms.Xaml

type StartPage() as this =
    inherit ContentPage()
    let titleLabel = Label(Text="Size grid: 4")


    let setupSlider () =
        let slider = Stepper (2.0, 5.0, 4.0, 1.0)
        slider.ValueChanged.Add (fun x ->
            let str = x.NewValue.ToString()
            titleLabel.Text <- "Size grid: " + str
        )
        (slider)


    let actionButton () =
        let value = titleLabel.Text
        let chara = value.[11].ToString() |> int
        this.Navigation.PushAsync (new MainPage (chara), true) |> ignore

    let setupButton () =
        let button = Button ()
        button.Clicked.Add (fun _ ->
            actionButton ()
        )
        button.CornerRadius <- 10
        button.BackgroundColor <- Color.Salmon
        button.TextColor <- Color.White
        button.Text <- "Transition"
        (button)

    let setupRootView () =
        let view = RelativeLayout ()
        view.BackgroundColor <- Color.Gray
        let button = setupButton ()
        let slider = setupSlider ()
        
        titleLabel.FontSize <- Device.GetNamedSize(NamedSize.Large, titleLabel)
        view.Children.Add (titleLabel,
            Constraint.RelativeToParent (fun x -> x.Width/2.0-55.0),
            Constraint.RelativeToParent (fun x -> x.X)
        )
        view.Children.Add (button,
            Constraint.RelativeToParent (fun x -> x.Width/2.0-60.0),
            Constraint.RelativeToParent (fun x -> x.Height/2.0-30.0),
            Constraint.Constant (120.0),
            Constraint.Constant (60.0)
        )
        view.Children.Add (slider,
            Constraint.RelativeToParent (fun x -> x.Width/2.0-47.0),
            Constraint.RelativeToView (button, fun _ x -> x.Y+75.0)
        )
        view.BackgroundColor <- Color.Beige
        (view)

    let initialize =
        base.Content <- setupRootView ()

