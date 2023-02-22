module FunctionsUI
open Xamarin.Forms

// UI for MainPage.xaml.fs

let createNewBox () =
    let baseFrame = Frame()
    baseFrame.Margin <- Thickness(5.0, 5.0, 5.0, 5.0)
    baseFrame.CornerRadius <- (float32) 8.0
    baseFrame.BackgroundColor <- Color.Salmon
    (baseFrame)


let createNewLabel myText =
    let newLabel = Label()
    newLabel.Text <- myText
    newLabel.TextColor <- Color.White
    newLabel.HorizontalOptions <- LayoutOptions.Center
    newLabel.VerticalOptions <- LayoutOptions.Center
    newLabel.FontSize <- Device.GetNamedSize(NamedSize.Medium, newLabel)
    newLabel

let updateGrid (list2: string[,]) (numberRows: int) (frames: Frame[,]) =
    for i in 0 .. (numberRows - 1) do
        for j in 0 .. (numberRows - 1) do
            let value = list2.[i, j]
            frames.[i, j].Content <- createNewLabel value

let setupGrid (numberRows: int) (frames: Frame[,]) =
    let grid = Grid()
    let rows = RowDefinitionCollection()
    let newRow = RowDefinition()
    newRow.Height <- GridLength(100.0, GridUnitType.Star)
    let columns = ColumnDefinitionCollection()
    let newColumn = ColumnDefinition()
    newColumn.Width <- GridLength(100.0, GridUnitType.Star)

    for _ in 0 .. (numberRows - 1) do
        rows.Add newRow
        columns.Add newColumn

    grid.RowDefinitions <- rows
    grid.ColumnDefinitions <- columns

    for i in 0 .. (numberRows - 1) do
        for j in 0 .. (numberRows - 1) do
            let newBox = createNewBox ()
            grid.Children.Add(newBox, i, j)
            frames.[j, i] <- newBox

    (grid)


// UI for StartPage.xaml.fs
let setupButton (actionButton: (unit -> unit)) =
    let button = Button()
    button.Clicked.Add(fun _ -> actionButton ())
    button.CornerRadius <- 10
    button.BackgroundColor <- Color.Salmon
    button.TextColor <- Color.White
    button.Text <- "Transition"
    (button)


let titleLabel = Label(Text = "Size grid: 4")

let setupStepper () =
    let stepper = Stepper(2.0, 5.0, 4.0, 1.0)

    stepper.ValueChanged.Add(fun x ->
        let str = x.NewValue.ToString()
        titleLabel.Text <- "Size grid: " + str)

    (stepper)


let setupRootView (actionButton: (unit -> unit)) =
    let view = RelativeLayout()
    view.BackgroundColor <- Color.Gray
    let button = setupButton (actionButton)
    let slider = setupStepper ()

    titleLabel.FontSize <- Device.GetNamedSize(NamedSize.Large, titleLabel)

    view.Children.Add(
        titleLabel,
        Constraint.RelativeToParent(fun x -> x.Width / 2.0 - 55.0),
        Constraint.RelativeToParent(fun x -> x.X)
    )

    view.Children.Add(
        button,
        Constraint.RelativeToParent(fun x -> x.Width / 2.0 - 60.0),
        Constraint.RelativeToParent(fun x -> x.Height / 2.0 - 30.0),
        Constraint.Constant(120.0),
        Constraint.Constant(60.0)
    )

    view.Children.Add(
        slider,
        Constraint.RelativeToParent(fun x -> x.Width / 2.0 - 47.0),
        Constraint.RelativeToView(button, (fun _ x -> x.Y + 75.0))
    )

    view.BackgroundColor <- Color.Beige
    (view)