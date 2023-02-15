namespace Lab4

open System
open Xamarin.Forms
open Xamarin.Forms.Xaml

type MainPage(numberX: int) as this =
    inherit ContentPage()
    let r = System.Random()
    let myList = Array2D.zeroCreate<Frame> numberX numberX
    let mutable elemsArr = Array2D.create (numberX) (numberX) ""


    let generateRandomNum () =
        let choosingValues = [|"2"; "4"|]
        let value = choosingValues.[(r.Next(0, 2))]
        (value)


    let createBox () =
        let newBox = new Frame()
        newBox.Margin <- new Thickness(5.0, 5.0, 5.0, 5.0)
        newBox.CornerRadius <- (float32)8.0
        newBox.BackgroundColor <- Color.Salmon
        newBox

    let createLab myText =
        let newLabel = new Label()
        newLabel.Text <- myText
        newLabel.TextColor <- Color.White
        newLabel.HorizontalOptions <- LayoutOptions.Center
        newLabel.VerticalOptions <- LayoutOptions.Center
        newLabel.FontSize <- Device.GetNamedSize(NamedSize.Medium, newLabel)
        newLabel

    let updateGrid (list2: string[,]) =
        for i in 0..(numberX-1) do
            for j in 0..(numberX-1) do
                let value = list2.[i, j]
                myList.[i, j].Content <- createLab value

    let checkEnd () =
        let oldValue = elemsArr
        updateGrid elemsArr
        if (oldValue = elemsArr) then
            this.Navigation.PopAsync () |> ignore
        ()

    let generateNewValue () =
        let freeBoxes = elemsArr |> Array2D.mapi (fun i j v ->
            match v with
            | "" -> (i,j)
            | _ -> (-1,-1)
        )
        let free: (int*int) list = freeBoxes |> Seq.cast<int*int> |> Seq.toList |> List.filter (fun els -> els <> (-1,-1))
        if (free.Length > 0) then
            let idx = r.Next(0, free.Length-1)
            let newValue = generateRandomNum ()
            let i, j = free.[idx]
            elemsArr.[i,j] <- newValue
            updateGrid elemsArr
        else
            checkEnd ()


    let rendomizeSetup () =
        generateNewValue ()
        generateNewValue ()


    let rec shifLeftUp (arr: string[]) (value: string) (i: int) =
        if (i < 0) || (arr.[i] <> "") then
            (arr)
        else
            arr.[i+1] <- ""
            arr.[i] <- value
            shifLeftUp arr value (i-1)

    let rec shiftRightDown (arr: string[]) (value: string) (i: int) =
        if (i > numberX-1) || (arr.[i] <> "") then
            (arr)
        else
            arr.[i] <- value
            arr.[i-1] <- ""
            shiftRightDown arr value (i+1)


    


    let myHandlerLeft () =
        let shift () =
            for i in 0..(numberX-1) do
                for j in 0..(numberX-1) do
                    if elemsArr.[i,j] <> "" then
                        elemsArr.[i,*] <- shifLeftUp elemsArr.[i,*] elemsArr.[i,j] (j-1)
            ()

        shift ()

        for i in 0..(numberX-1) do
            for j in 0..(numberX-2) do
                if (elemsArr.[i,j] = elemsArr.[i,j+1]) && (elemsArr.[i,j] <> "") then
                    let value = elemsArr.[i,j] |> int
                    elemsArr.[i,j] <- (value*2).ToString()
                    elemsArr.[i,j+1] <- ""

        shift ()
        updateGrid elemsArr


    let handlerRight () =
        let shift () =
            for i in 0..(numberX-1) do
                for j in (numberX-1) .. -1 .. 0 do
                    if (elemsArr.[i,j] <> "") then
                        elemsArr.[i,*] <- shiftRightDown elemsArr.[i,*] elemsArr.[i,j] (j+1)

        shift ()

        for i in 0..(numberX-1) do
            for j in (numberX-1) .. -1 .. 1 do
                if (elemsArr.[i,j] = elemsArr.[i,j-1]) && (elemsArr.[i,j] <> "") then
                    let value = elemsArr.[i,j] |> int
                    elemsArr.[i,j] <- (value*2).ToString()
                    elemsArr.[i,j-1] <- ""

        shift ()

        updateGrid elemsArr

    let handlerUp () =
        let shift () =
            for i in 0..(numberX-1) do
                for j in 0..(numberX-1) do
                    if (elemsArr.[j,i] <> "") then
                        elemsArr.[*,i] <- shifLeftUp elemsArr.[*,i] elemsArr.[j,i] (j-1)

        shift ()

        for i in 0..(numberX-1) do
            for j in 0..(numberX-2) do
                if (elemsArr.[j,i] = elemsArr.[j+1,i]) && (elemsArr.[j,i] <> "") then
                    let value = elemsArr.[j,i] |> int
                    elemsArr.[j,i] <- (value*2).ToString()
                    elemsArr.[j+1,i] <- ""

        shift ()

        updateGrid elemsArr

    let handlerDown () =
        let shift () =
            for i in 0..(numberX-1) do
                for j in (numberX-1) .. -1 .. 0 do
                    if (elemsArr.[j,i] <> "") then
                        elemsArr.[*,i] <- shiftRightDown elemsArr.[*,i] elemsArr.[j,i] (j+1)

        shift ()

        for i in 0..(numberX-1) do
            for j in (numberX-1) .. -1 .. 1 do
                if (elemsArr.[j,i] = elemsArr.[j-1,i]) && (elemsArr.[j,i] <> "") then
                    let value = elemsArr.[j,i] |> int
                    elemsArr.[j,i] <- (value*2).ToString()
                    elemsArr.[j-1,i] <- ""

        shift ()

        updateGrid elemsArr
            

    let addRecogrinzers (grid: Grid) (myList: Frame [,]) =
        let leftRecognizer = SwipeGestureRecognizer(Direction=SwipeDirection.Left)
        leftRecognizer.Swiped.Add(fun _ ->
            myHandlerLeft ()
            generateNewValue ()
        )
        let rightRecognizer = SwipeGestureRecognizer(Direction=SwipeDirection.Right)
        rightRecognizer.Swiped.Add(fun _ ->
            handlerRight ()
            generateNewValue ()
        )
        let upRecognizer = SwipeGestureRecognizer(Direction=SwipeDirection.Up)
        upRecognizer.Swiped.Add(fun _ ->
            handlerUp ()
            generateNewValue ()
        )
        let downRecognizer = SwipeGestureRecognizer(Direction=SwipeDirection.Down)
        downRecognizer.Swiped.Add(fun _ ->
            handlerDown ()
            generateNewValue ()
        )
        grid.GestureRecognizers.Add leftRecognizer
        grid.GestureRecognizers.Add rightRecognizer
        grid.GestureRecognizers.Add upRecognizer
        grid.GestureRecognizers.Add downRecognizer

    let setupGrid () =
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
                let newBox = createBox ()
                grid.Children.Add(newBox, i, j)
                myList.[j,i] <- newBox
        rendomizeSetup ()

        addRecogrinzers grid myList
        (grid)

    let setupRootView () =
        let view = new RelativeLayout ()
        view.BackgroundColor <- Color.Beige
        let grid = setupGrid ()
        view.Children.Add (grid,
            Constraint.Constant (10.0),
            Constraint.Constant (0.0),
            Constraint.RelativeToParent (fun x -> x.Width-20.0),
            Constraint.RelativeToParent (fun x -> x.Width-20.0)
        )
        (view)


    let baseViewLoad =
        base.Content <- setupRootView ()

        