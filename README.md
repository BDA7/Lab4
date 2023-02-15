# Функциональное программирование
## Лабораторная работа 4: "2048 на iOS"

**Выполнил:** Бондаренко Данила Александрович \
**Группа:** P34112 \
**Преподаватель:** Пенской Александр Владимирович

### Реализация:
При загрузке приложения мы видим стартовый экран(StartPage), на нем Stepper(размером от 2 до 5) для изменения размерности сетки, Label с выводом для пользователя размера
и кнопка перехода на экран с игрой. Посмотреть логику экрана можно в файле StartPage.xaml.fs 

При переходе на экран с игрой(MainPage), видно саму сетку(grid) с заранее выбраным размером для нее(на первом экране), также на экране работают свайпы для изменения grid
при свайпе влево мы складываем все эелемнты влево, потом суммируем если есть равные элементы рядом и снова складываем их влево чтобы убрать между получеными числами пробелы, а далее обновляем модель.

Обработчики свайпов:
```f#
let addRecogrinzers (grid: Grid) =
        let leftRecognizer = SwipeGestureRecognizer(Direction = SwipeDirection.Left)

        leftRecognizer.Swiped.Add(fun _ ->
            myHandlerLeft ()
            generateNewValue ())

        let rightRecognizer = SwipeGestureRecognizer(Direction = SwipeDirection.Right)

        rightRecognizer.Swiped.Add(fun _ ->
            handlerRight ()
            generateNewValue ())

        let upRecognizer = SwipeGestureRecognizer(Direction = SwipeDirection.Up)

        upRecognizer.Swiped.Add(fun _ ->
            handlerUp ()
            generateNewValue ())

        let downRecognizer = SwipeGestureRecognizer(Direction = SwipeDirection.Down)

        downRecognizer.Swiped.Add(fun _ ->
            handlerDown ()
            generateNewValue ())

        grid.GestureRecognizers.Add leftRecognizer
        grid.GestureRecognizers.Add rightRecognizer
        grid.GestureRecognizers.Add upRecognizer
        grid.GestureRecognizers.Add downRecognizer
```

Пример реализации свайпа влево:
```f#
let rec shifLeftUp (arr: string[]) (value: string) (i: int) =
        if (i < 0) || (arr.[i] <> "") then
            (arr)
        else
            arr.[i + 1] <- ""
            arr.[i] <- value
            shifLeftUp arr value (i - 1)
            

let myHandlerLeft () =
        let shift () =
            for i in 0 .. (numberX - 1) do
                for j in 0 .. (numberX - 1) do
                    if elemsArr.[i, j] <> "" then
                        elemsArr.[i, *] <- shifLeftUp elemsArr.[i, *] elemsArr.[i, j] (j - 1)

            ()

        shift ()

        for i in 0 .. (numberX - 1) do
            for j in 0 .. (numberX - 2) do
                if (elemsArr.[i, j] = elemsArr.[i, j + 1]) && (elemsArr.[i, j] <> "") then
                    let value = elemsArr.[i, j] |> int
                    elemsArr.[i, j] <- (value * 2).ToString()
                    elemsArr.[i, j + 1] <- ""

        shift ()
        updateGrid elemsArr
```
Реализации свайпов вверх, вниз и вправо немного отличаются, но идея одна.

Реализация обновления модели и UI:
```f#
let updateGrid (list2: string[,]) =
        for i in 0 .. (numberX - 1) do
            for j in 0 .. (numberX - 1) do
                let value = list2.[i, j]
                myList.[i, j].Content <- createLab value
```
Так как у нас хранятся ссылки на каждую ячейку в сетке, мы можем обновлять их по ссылке заменяя контент на новый, относительно нашей модели.

При загрузе экрана срабатывает функция для генерации двух чисел из набора 2 и 4, и их расположению на сетке(выбираются случайно).
```f#
    let rendomizeSetup () =
        generateNewValue ()
        generateNewValue ()
```

Генератор чисел:
```f#
let generateRandomNum () =
        let choosingValues = [| "2"; "4" |]
        let value = choosingValues.[(r.Next(0, 2))]
        (value)
        
        
let generateNewValue () =
        let freeBoxes =
            elemsArr
            |> Array2D.mapi (fun i j v ->
                match v with
                | "" -> (i, j)
                | _ -> (-1, -1))

        let free: (int * int) list =
            freeBoxes
            |> Seq.cast<int * int>
            |> Seq.toList
            |> List.filter (fun els -> els <> (-1, -1))

        if (free.Length > 0) then
            let idx = r.Next(0, free.Length - 1)
            let newValue = generateRandomNum ()
            let i, j = free.[idx]
            elemsArr.[i, j] <- newValue
            updateGrid elemsArr
        else
            checkEnd ()
```

Расположение сетки на экране задается относительно размера нашего устройства:
```f#
let setupRootView () =
        let view = RelativeLayout()
        view.BackgroundColor <- Color.Beige
        let grid = setupGrid ()

        view.Children.Add(
            grid,
            Constraint.Constant(10.0),
            Constraint.Constant(0.0),
            Constraint.RelativeToParent(fun x -> x.Width - 20.0),
            Constraint.RelativeToParent(fun x -> x.Width - 20.0)
        )

        (view)
```


## Тесты для функций лежат тут: https://github.com/BDA7/Test2048App
