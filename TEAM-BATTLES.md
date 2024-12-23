# Командные битвы

Обычно на хакатонах программисты борятся друг с другом, разрабатывая программы на скорость.

Но в реальной жизни нам редко нужно «воевать» и работать в одиночку.
Мы пишем код в команде, при этом сотрудничаем с другими командам — фронт с беком, бек с мобильщиками.
И хороший результат нашей работы — программа, написанная в соответствии с контрактом, которая с первого раза заработала с другой — парной — программой.

Впервые мы провели *Командные битвы* 16 марта 2024 года на [IT Purple Conf](https://fpmiconf.ru/) — конференции для студентов и школьников, которую проводит [Физтех-школа прикладной математики и информатики МФТИ](https://pk.mipt.ru/schools/fpmi/).

Затем добавили задачи, так, что теперь можно выбрать несколько задач и устроить развлечение хоть на целый день.

25 мая 2024 года прошёл марафон [Т1.Код.Май](https://codenrock.com/contests/kod-marafon-t1-may#/) на платформе [Codenrock](https://codenrock.com/#/).
Задачи ещё раз прошли обкатку в боевых условиях.
Формат не позволили разбивать участников на *красные* и *синие* команды, поэтому формулировка задач [изменилась](doc/) — студенты последовательно решали две части одной задачи.

## Механика игры

Участники разбиваются на команды (не меньше двух).
Каждая команда выбирает язык программирования, на котором будет писать программу.
У каждой команды есть ноутбук, подключенный к интернету.

Язык программирования может быть любым.
Мы будем писать консольное приложение, которое читает из входного потока и печатает в выходной поток.
На ноутбуке должна быть настроена среда разработки (IDE) для выбранного языка, чтобы не тратить драгоценное время на настройку.

В начале игры все команды делятся на два типа: *красные* и *синие*.
Не обязательно, чтобы их было поровну, но желательно всё-таки соблюсти баланс.
Все получают задания (см. ниже) и подключаются к Tg-чату, адрес которого выведен на проекторе.
Tg-чат нужен для обмена результатами. В нём сидят организаторы и все команды.

Важно отметить, что красные и синие не соревнуются друг с другом, а решают две части одной задачи.
Только если и те, и другие всё сделают правильно, задача будет решена.

Команды выполняют задание в режиме mob-programming, то есть задачу обсуждают вместе, а код при этом пишет кто-то один.
Человек, который пишет (ведущий) может меняться.
Чтобы все могли участвовать, важно, чтобы команды были не слишком большими.
С другой стороны, в маленькой команде не удастся охватить все роли.
Оптимально, если все команды состоят из 4-7 человек.

Можно начинать игру, когда своё задание завершает одна красная и одна синяя команда.

Организаторы сбрасывают в чат тестовые задания для красных, красные отправляют её на вход своей программе, результат копируют в чат.
Синие отправляют результат красных на вход своей программе и тоже копируют в чат то, что получилось.
Если всё сделано правильно, на выходе будут такие же данные, как и на входе.

Если в игре принимают участие несколько красных и синих команд, они играют попарно.
Если возникают ошибки, команда может их исправить в пределах отведённого времени.

После игры даём возможность участникам поделиться впечатлениями и просим выбрать 1 или 2 человек из каждой команды, кого хотелось бы отметить.
Просим рассказать, как именно они помогали команде.
В финале награждаем отмеченных людей призами.

## Технические требования

### До проведения

* Создать Tg-чат
* Подготовить слайд с qr-кодом чата для вывода на экран
* Подготовить и распечатать задания
* Подготовить тестовые данные

### Площадка

* Интернет на площадке
* Проектор/экран

### Тайминг

В начале выделяем 15–20 минут, чтобы рассказать участникам о правилах, разбить их на команды и поделить на *красных* и *синих*.

Ориентировочное время решения задач см. в комментариях к задаче.

После каждой задачи даём участникам возможность поделиться впечатлениями.
Не забываем про кофе-брейки.
В общей сложности, на перерыв планируйте 15 минут.

### Ссылки

* [ASCII Art](https://www.asciiart.eu/image-to-ascii) — сайт с готовыми образцами ASCII-графики и онлайн-утилитой, которая поможет подготовить свои пример.

## Задачи

Все тестовые данные должны быть в формате ASCII text.
Современные текстовые редакторы, как правило, работают с UTF-8, поэтому проверьте, что в файлах встречаются только ASCII-символы и нет BOM.

### Шифр Цезаря

Планируемое время: 30 минут.

[Исходный код](src/caesar/).

**forrest-gump-decoded.txt**
```text
18
Mama always said life was like a box of chocolates. You never know what you're gonna get.
```

Кодирование файла:

```bash
dotnet run -- encode < forrest-gump-decoded.txt
```

Поместив вывод в **forrest-gump-encoded.txt** и добавив 18 в первую строку, получим:

**forrest-gump-encoded.txt**
```text
18
Eses sdosqk ksav daxw osk dacw s tgp gx uzgugdslwk. Qgm fwnwj cfgo ozsl qgm'jw ygffs ywl.
```

```bash
dotnet run -- decode < forrest-gump-encoded.txt
```

[Задача для красных (PDF)](https://github.com/markshevchenko/team-battles/files/15241145/caesar-for-reds.pdf) \
[Задача для синих (PDF)](https://github.com/markshevchenko/team-battles/files/15241148/caesar-for-blues.pdf)

### ASCII Art

Планируемое время: 60 минут.

[Исходный код](src/ascii-art/).

**einstein-decoded.txt**
```text
      -''--.
       _`>   `\.-'<
    _.'     _     '._
  .'   _.='   '=._   '.
  >_   / /_\ /_\ \   _<
    / (  \o/\\o/  ) \
    >._\ .-,_)-. /_.<
jgs     /__/ \__\ 
          '---'     E=mc^2
```

```bash
dotnet run -- encode < einstein-decoded.txt
```

**einstein-encoded.txt**
```text
9
26
R6P-RP'RP'RP-RP-RP.L11DR7P_RP`RP>R4P`RP\RP.RP-RP'RP<L18DR4P_RP.RP'R6P_R6P'RP.RP_L20DR2P.RP'R4P_RP.RP=RP'R4P'RP=RP.RP_R4P'RP.L22DR2P>RP_R4P/R2P/RP_RP\R2P/RP_RP\R2P\R4P_RP<L22DR4P/R2P(R3P\RPoRP/RP\RP\RPoRP/R3P)R2P\L20DR4P>RP.RP_RP\R2P.RP-RP,RP_RP)RP-RP.R2P/RP_RP.RP<L20DR0PjRPgRPsR6P/RP_RP_RP/R2P\RP_RP_RP\L16DR10P'RP-RP-RP-RP'R6PERP=RPmRPcRP^RP2
```

```bash
dotnet run -- decode < einstein-encoded.txt
```

[Задача для красных (PDF)](https://github.com/markshevchenko/team-battles/files/15241123/ascii-art-for-reds.pdf) \
[Задача для синих (PDF)](https://github.com/markshevchenko/team-battles/files/15241126/ascii-art-for-blues.pdf)

### Система счисления

Планируемое время: 60 минут.

[Исходный код](src/number-system/).

**ex1_decoded.txt**
```text
1234567890
```

```bash
dotnet run -- encode < ex1_decoded.txt
```

**ex1_encoded.txt**
```text
124edcbaa0
```

```bash
dotnet run -- decode < ex1_encoded.txt
```

[Задача для красных (PDF)](https://github.com/markshevchenko/team-battles/files/15241156/number-system-for-reds.pdf) \
[Задача для синих (PDF)](https://github.com/markshevchenko/team-battles/files/15241155/number-system-for-blues.pdf)

### Хаффман

Планируемое время: 90 минут.

[Исходный код](src/huffman/).

Сначала готовим дерево Хаффмана:

```bash
dotnet run -- prepare forrest_gump_decoded.txt
```

**forrest_gump_huffman.txt**
```text
53
P 
Ph
Pg
C
Pf
Pm
Pd
C
C
C
Pc
Pb
P'
C
C
P.
Pt
C
C
C
C
Pa
Pi
Py
PY
Px
C
C
C
Pe
C
C
Pw
Ps
C
Po
C
Pu
Pr
C
Pn
C
Pl
Pv
PM
C
Pk
C
C
C
C
C
C
```

```bash
cat forrest_gump_huffman.txt forrest_gump_decoded.txt | dotnet run -- encode
```

**forrest_gump_encoded.txt**
```text
1111101100010110100001001111011000100101010110010011001100101000101110011110101000101010110011000100110010011110101001111111011001000001101011011010111001101010100001100010001101011001101111101000111110111100101110001010110110111100000111011011111110010111110010011111111101110111000001100001000100011110010101011011110000110111110011011000100111011110111101100000100110110111101110
```

```bash
cat forrest_gump_huffman.txt forrest_gump_encoded.txt | dotnet run -- decode
```

[Задача для красных (PDF)](https://github.com/markshevchenko/team-battles/files/15241160/huffman-for-reds.pdf) \
[Задача для синих (PDF)](https://github.com/markshevchenko/team-battles/files/15241159/huffman-for-blues.pdf)
