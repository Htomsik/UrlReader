<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a>
    <img src="https://raw.githubusercontent.com/Htomsik/Htomsik/main/Assets/AllTestTasks/TestIcon.png" alt="Logo" width="200">
  </a>

</div>

<h3 align="center">UrlReader (Тестовое задание)</h3>

<div align="left">
  <ol>
    <li>
      <a href="#О-проекте">О проекте</a>
      <ul>
        <li><a href="#Краткая-сводка">Краткая сводка</a></li>
      </ul>
      <ul>
        <li><a href="#Установка">Установка</a></li>
      </ul>
      <ul>
        <li><a href="#Использованные-библиотеки">Использованные библиотеки</a></li>
      </ul>
    </li>
    <li><a href="#Функционал">Функционал</a></li>
      <ul>
        <li><a href="#Интерфейс">Интерфейс</a></li>
      </ul>
      <ul>
        <li><a href="#Видео-демонстрация">Видео-демонстрация</a></li>
      </ul>
      <ul>
        <li><a href="#Структура-приложения">Структура приложения</a></li>
      </ul>
    <li><a href="#Возникшие-сложности">Возникшие сложности и как решал</a></li>
    <li><a href="#Не-решённые-на-данный-момент-проблемы">Не решенные проблемы</a></li>
    <li><a href="#Возможные-улучшения-функционала">Возможные улучшения функционала</a></li>
    <li><a href="#Связь-со-мной">Связь со мной</a></li>
  </ol>
</div>



<!-- ABOUT THE PROJECT -->
<div align="center">

# О проекте



<img width="80%" src="https://raw.githubusercontent.com/Htomsik/UrlReader/master/Assets/Preview/Prewiew.png"/>

</div>

## Краткая сводка

<h3 aligh="right">UrlRead - Программа для считывания ссылок на веб страницы из файла в формате Json и ведения некоторой статистики по ним.</h2>

## Использованные библиотеки

| Библиотека                                                                                          | Как использовал                                                                                                                                                |
|-----------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [![Serilog][.Serilog-shield]][.Serilog-url]                                                         | Лоигрование в файл и интерфейс для отображения выполняемых действий                                                                                            |
| [![FastJson][.FastJson-shield]][.FastJson-url]                                                      | Для десериализации данных из загружаемых файлов                                                                                                                |
 | [![AngleSharp][.AngleSharp-shield]][.AngleSharp-url]                                                | Для парсинга тегов с html страницы                                                                                                                             |
| [![ReactiveUi][.ReactiveUi-shield]][.ReactiveUi-url]                                                | Для создания базовой инфраструктуры mvvm приложения                                                                                                            |
| [![LiteApp][.LiteApp-shield]][.LiteApp-url]                                                         | Для создания базовой инфраструктуры хранилищ и сервисов (Собственная библиотека, возникли некоторые сложности, но в целом потихоньку расширяется и улучшается) |
| [![Microsoft.Extension.Hosting][.Microsoft.Extension.Hosting-shield]][.Microsoft.Extension.Hosting-url] | Для создания хоста приложения и DI контейнера                                                                                                                  |
| [![MaterialDesignThemes][.MaterialDesignThemes-shield]][.MaterialDesignThemes-url]                  | Для дизайна WPF (Готовая xaml тема)                                                                                                                            |



## Установка

**Последнюю версию установщика, а также пояснения по запуску находятся [тут](https://github.com/Htomsik/UrlReader/releases)**

<div align="center">

## Функционал

### Интерфейс

<img src="https://raw.githubusercontent.com/Htomsik/UrlReader/master/Assets/Preview/Functional%20Interface%20diagram.png"/>


### Видео-демонстрация


<a href="https://www.youtube.com/watch?v=1EPAk54lnmY&t=7s">
<img width="60%" src="https://raw.githubusercontent.com/Htomsik/UrlReader/master/Assets/Preview/VideoPrewiew.png"/>
</a>

<div align="left">

</div>



 ### Структура приложения

</div>

Особенности структуры работы с данными:

* Хранилища - зарегистрированные в IOC re-used контейнеры с данными. Базовые реализации и пояснения как работают определены в [LiteApp][.LiteApp-url]
* Сервисы - обьекты управляющие данными, зачастую в хранилишах.

Особенности реализации mvvm:
* Модели - в данном случае модели реализуют INPC для отображения их изменений в интерфейсе
* ViewModel (дальше: vmd) - обычно я разделю vmd на два типа:
  * Управляющие отображением других vmd.
  > Например если бы я готовил это приложение с учётом того что в будующем оно будет обрастать функционалом 
  > то блоки показанные на схеме выше были бы отдельными UserControl со своими vmd, а управлением этими блокми бы занимался отдельный vmd.
  * Упавляющий отображением данных.
  > В данном случае такими блоками были бы: Статистика, таблица с данными и верхнее меню.

Но в этот раз я точно знал какой функционал должен быть в приложении, поэтому не стал создавать навигационную инфраструктуру.


<div align="center">

## Возникшие сложности

</div>

### Утечка памяти (Частичное решение)

<details>
  <summary>Описание внутри</summary>

**Причина:** Изначальное незнание правильной работы с подписками в ReactiveUi (Уже понял что делал не так)

**Как решал:** с помощью dotMemory находил моменты когда потребляемая память вела себя ненормально и смотрел что вызывалось в этот момент, также пользовался разными средствами в Rider.

**Удалось решить?:** Основная утечка устранена, однако периодически из за неправильного использования ReactiveUi всплывают небольшие утечки, а также данные после очистки стираются не сразу.

</details>


### Ускорение алгоритмов загрузки данных в приложение и парсинга (Удалось решить)

<details>
 <summary>Описание внутри</summary>

**Причина:** Первая версия приложения была асинхронная, но операции с данными выполнялись в одном потоке.

**Как решал:** Добавил для загрузки из json файла асинхронную передачу из стрима и сменил библиотеку для десериализации, а сам парсинг разбил на блоки по 100 потоков.

**Удалось решить?:** Скорость ввода и обработки данных значительно повысилась, однако для парсинга наложились проблемы с http (о них ниже)

</details>


### Пробемы с Http (Частичное решение)

<details>
 <summary>Описание внутри</summary>

**Суть:** http оставляет после себя открытые соединения, настройки хандлеров для http не приносили успеха

**Как решал:** Ограничил одновременных запросов до 100 за раз, а также снизил TimeOut запроса до нескольких секунд и для компенсации количества необработанных соединений добавил несколько подходов на один блок запрашиваемых данных.

**Удалось решить?:** Частично. Количество успешно обработанных соединений (любые не Unknown статусы) возросло в несколько раз, однако общий покаатель отработанных запросов находиться в районе 80%.

</details>


<div align="center">

## Не решенные на данный момент проблемы

</div>

### Провисания во время отображения больших массивов данных в таблице

<details>
 <summary>Описание внутри</summary>

**Причина:** Загрузка одновременно свыше 100к данных в таблицу.

**Степень критичности:** Низкая, провисание на несколько секунд.

**Возможное решение:** Разбивка большого массива на блоки по 10к и загрузка по очереди

**Почему сразу не исправил:** Ограничение по времени. Реализация хранилищ в LiteApp не асинхронная и некоторые операции можно будет сделать только после доработки в библиотеке.

</details>

### Проблемы асинхонных обращений к хранилищам

<details>
 <summary>Описание внутри</summary>

**Причина:** Отсуствие реализации асинхронных хранилищ в LiteApp

**Степень критичности:** Критическое. Многие функции приложения были урезаны или не могут быть реализованны по этой причине

**Затронутые сферы:** 

* Обновление у сервиса статискики происходит только в моменты когда хранилище никто не использует
* Невозможность исправить провисания при загрузке больших данных
* Невозможность использования хранилища для сообщений от логгера в многопоточном режиме

**Почему сразу не исправил:** Ограничение по времени. Реализация хранилищ в LiteApp не асинхронная и некоторые операции можно будет сделать только после доработки в библиотеке.

</details>


<div align="center">

## Возможные улучшения функционала

</div>

### Если решить вышеописанные проблемы то можно было бы

* Добавить вывод последних 50 сообщений от логгера в отдельной таблице как возможную функцию
* Добавить одновременную загрузка из файла, десериализацию и запуск парсинга.
* Значительно расширить статистику
* Выводить в разные таблицы отфильтрованные данные по различным параметрам (по выбору)
* Добавить фильтры для парсинга: максимальный вес скачиваемого сайта, время ожидания и тд.
* Добавить drag and drop для файлов и т.д.


### Что требуется улучшить

* Алгоритм проверки данных в файле (сейчас он очень простой и требует соблюдения от пользователя конкретных норм)
* Код написанный криворукими руками (моими)


# Связь со мной

Почта:lhz4ru822@mozmail.com

[![Konstantin](https://img.shields.io/badge/Konstantin-090909?style=for-the-badge&logo=vk&logoColor=red)](https://vk.com/jessnjake)



[.Serilog-shield]: https://img.shields.io/badge/Serilog-090909?style=for-the-badge&logo=
[.Serilog-url]: https://serilog.net/

[.AngleSharp-shield]: https://img.shields.io/badge/AngleSharp-090909?style=for-the-badge&logo=
[.AngleSharp-url]: https://anglesharp.github.io/

[.FastJson-shield]: https://img.shields.io/badge/FastJson-090909?style=for-the-badge&logo=
[.FastJson-url]: https://www.codeproject.com/Articles/159450/fastJSON-Smallest-Fastest-Polymorphic-JSON-Seriali

[.ReactiveUi-shield]: https://img.shields.io/badge/ReactiveUi-090909?style=for-the-badge&logo=
[.ReactiveUi-url]: https://www.reactiveui.net/

[.ReactiveUi-shield]: https://img.shields.io/badge/ReactiveUi-090909?style=for-the-badge&logo=
[.ReactiveUi-url]: https://www.reactiveui.net/

[.Microsoft.Extension.Hosting-shield]: https://img.shields.io/badge/Microsoft_Extension_Hosting-090909?style=for-the-badge&logo=
[.Microsoft.Extension.Hosting-url]: https://dotnet.microsoft.com/en-us/

[.MaterialDesignThemes-shield]: https://img.shields.io/badge/Material_Design_Themes-090909?style=for-the-badge&logo=
[.MaterialDesignThemes-url]: https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit

[.LiteApp-shield]: https://img.shields.io/badge/Lite_App-090909?style=for-the-badge&logo=
[.LiteApp-url]: https://github.com/Htomsik/LiteApp


