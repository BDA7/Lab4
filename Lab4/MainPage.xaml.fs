namespace Lab4

open System
open Xamarin.Forms
open Xamarin.Forms.Xaml

type MainPage() =
    inherit ContentPage()
    let numberX = 5
    let rootView = StackLayout()
    let myList = Array2D.zeroCreate<Frame> numberX numberX
    let mutable elemsArr = Array2D.create (numberX+1) (numberX+1) ""

    let createBox color =
        let newBox = new Frame()
        newBox.Margin <- new Thickness(5.0, 5.0, 5.0, 5.0)
        newBox.CornerRadius <- (float32)8.0
        newBox.BackgroundColor <- color
        newBox

    let createLab myText =
        let newLabel = new Label()
        newLabel.Text <- myText
        newLabel.TextColor <- Color.Brown
        newLabel.HorizontalOptions <- LayoutOptions.Center
        newLabel.VerticalOptions <- LayoutOptions.Center
        newLabel.FontSize <- Device.GetNamedSize(NamedSize.Small, newLabel)
        newLabel

    let updateGrid (list2: string[,]) =
        for i in 0..(numberX-1) do
            for j in 0..(numberX-1) do
                let value = list2.[i, j]
                myList.[i, j].Content <- createLab value

    let rendomizeSetup (myList: Frame [,]) =
        let r = System.Random()
        let startValues = [|"2"; "4"|]
        let chooseStart = startValues.[(r.Next(0, 2))]
        let num1 = r.Next(0, numberX-1)
        let num2 = r.Next(0, numberX-1)
        elemsArr.[num1,num2] <- chooseStart
        elemsArr.[num1+1,num2] <- chooseStart
        elemsArr.[num1+2,num2] <- chooseStart
        elemsArr.[num1+3,num2] <- chooseStart
        updateGrid elemsArr


    let rec shifLeft (arr: string[]) (value: string) (i: int) =
        if (i < 0) || (arr.[i] <> "") then
            (arr)
        else
            arr.[i+1] <- ""
            arr.[i] <- value
            shifLeft arr value (i-1)


    let myHandlerLeft () =
        for i in 0..(numberX-1) do
            for j in 0..(numberX-1) do
                if (elemsArr.[i,j] = elemsArr.[i,j+1]) && (elemsArr.[i,j] <> "") then
                    let value = elemsArr.[i,j] |> int
                    elemsArr.[i,j] <- (value*2).ToString()
                    elemsArr.[i,j+1] <- ""
        for i in 0..(numberX-1) do
            for j in 0..(numberX-1) do
                if elemsArr.[i,j] <> "" then
                    elemsArr.[i,*] <- shifLeft elemsArr.[i,*] elemsArr.[i,j] (j-1)
        updateGrid elemsArr


    let rec shiftRight (arr: string[]) (value: string) (i: int) =
        if (i >= numberX) || (arr.[i] <> "") then
            printfn "%A" arr
            (arr)
        else
            arr.[i-1] <- ""
            arr.[i] <- value
            shiftRight arr value (i+1)

    let handlerRight () =
        let rec iteration (i: int) =
            let rec insideIteration (j: int) =
                if (j <= 0) then
                    ()
                else
                    if (elemsArr.[i,j] = elemsArr.[i,j-1]) && (elemsArr.[i,j] <> "") then
                        let value = elemsArr.[i,j] |> int
                        elemsArr.[i,j] <- (value*2).ToString()
                        elemsArr.[i,j-1] <- ""
                        insideIteration (j-1)
                    else
                        insideIteration (j-1)

            if (i < 0) then
                ()
            else
                insideIteration (numberX-1)
                iteration (i-1)
        iteration (numberX-1)
        for i in 0..(numberX-1) do
            for j in (numberX-1) .. -1 .. 1 do
                if (elemsArr.[i,j] <> "") then
                    elemsArr.[i,*] <- shiftRight elemsArr.[i,*] elemsArr.[i,j] (j+1)
        updateGrid elemsArr

    let handlerUp () =
        for i in 0..(numberX-1) do
            for j in 0..(numberX-1) do
                if (elemsArr.[j,i] = elemsArr.[j+1,i]) && (elemsArr.[j,i] <> "") then
                    let value = elemsArr.[j,i] |> int
                    elemsArr.[j,i] <- (value*2).ToString()
                    elemsArr.[j+1,i] <- ""

        updateGrid elemsArr
            

    let addRecogrinzers (grid: Grid) (myList: Frame [,]) =
        let leftRecognizer = SwipeGestureRecognizer(Direction=SwipeDirection.Left)
        leftRecognizer.Swiped.Add(fun _ ->
            myHandlerLeft ()
        )
        let rightRecognizer = SwipeGestureRecognizer(Direction=SwipeDirection.Right)
        rightRecognizer.Swiped.Add(fun _ ->
            handlerRight ()
        )
        let upRecognizer = SwipeGestureRecognizer(Direction=SwipeDirection.Up)
        upRecognizer.Swiped.Add(fun _ ->
            handlerUp ()
        )
        let downRecognizer = SwipeGestureRecognizer(Direction=SwipeDirection.Down)
        downRecognizer.Swiped.Add(fun _ ->
            myList.[0, 0].Content <- createLab "7"
        )
        grid.GestureRecognizers.Add leftRecognizer
        grid.GestureRecognizers.Add rightRecognizer
        grid.GestureRecognizers.Add upRecognizer
        grid.GestureRecognizers.Add downRecognizer

    let clr =
        let grid = new Grid()
        let rows = new RowDefinitionCollection()
        let newRow = new RowDefinition()
        newRow.Height <- GridLength(100.0, GridUnitType.Star)
        let columns = new ColumnDefinitionCollection()
        let newColumn = new ColumnDefinition()
        newColumn.Width <- GridLength(100.0, GridUnitType.Star)
        for i in 0..(numberX-1) do
            rows.Add newRow
            columns.Add newColumn
        grid.RowDefinitions <- rows
        grid.ColumnDefinitions <- columns

        for i in 0..(numberX-1) do
            for j in 0..(numberX-1) do
                let newBox = createBox Color.Beige
                grid.Children.Add(newBox, i, j)
                myList.[j,i] <- newBox
        rendomizeSetup myList

        addRecogrinzers grid myList
        grid.Padding <- new Thickness(10.0, 250.0, 10.0, 250.0)
        base.Content <- grid

        