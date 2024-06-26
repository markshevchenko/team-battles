﻿# Алгоритм Хаффмана (кодирование)

Реализовать кодирование текста по алгоритму Хаффмана.

Алгоритм Хаффмана состоит из двух этапов: построение дерева Хаффмана и кодирование.
В этой задаче дерево уже построено, так что вам нужно реализовать только кодирование.

## Дерево Хаффмана

![Дерево Хаффмана](huffman_abc.svg)

Дерево Хаффмана — двоичное.
Все его узлы — либо *листья*, либо *ветвления*.
Каждому листу сопоставлен один печатный ASCII-символ.
Листья не имеют дочерних узлов.
Ветвления имеют ровно два дочерних узла.

## Вход программы

В первой строке программа получается целое число `N`, `3 ≤ N ≤ 100`.
Далее следуют `N` строк такого формата:

* Символ **P** (*англ.* push) за которым записан ровно один печатный символ ASCII.
  Например, `Pa`, `P-`, `PP`.
  Встретив эту команду, вы должны создать узел типа *лист*.
  Второй символ строки становится значением листа.
  Созданный лист помещается в стек.
* Символ **C** (*англ.* combine).
  Встретив эту команду, выдолжны извлечь из стека два узла и создать узел типа *ветвление*.
  Первый извлечённый узел будет правым дочерним узлом, а второй — левым.
  Созданное ветвление помещается в стек.

После `N` строк, содержащих команды, следует последняя строка, в которой находится текст для кодироваания.

**Пример входа программы**
```text
5
Pa
Pb
Pc
C
C
ababcaca
```

Эти команды строят дерево, показанное на рисунке.

## Кодирование

Мы должны закодировать каждый символ во входной строке.
В нашем примере первый символ это `a`.

Прежде, чем приступить к кодированию, пометим каждую левую ветвь дерева Хаффмана цифрой `0`, а каждую правую — цифрой `1` (на рисунке это уже сделано, так что вы можете с ним сверяться).

Составим маршрут от корня дерева до символа `a`.
Он состоит из одной левой ветви, помеченной цифрой `0`.
Это значит, что символу `a` соответствует код `0`.

Печатаем символ (не бит) `0`.

Следующий символ — `b`.
Маршрут из корня к листу `b` состоит из правой ветки (помечена цифрой `1`), сразу за которой следует левая ветка (помечена `0`).

Таким образом, код символа `b` — это последовательность `10`, которую мы печатаем без разделителей, то есть без пробелов, запятых, переводов строк и т.д.

Мы закодировали символы `ab`, напечатав последовательность `010`.
Далее символы повторяются, поэтому строке `abab` будет соответствовать код `010010`.

Затем мы встречаем символ `c`.
Построив в дереве путь от корня до символа `c`, мы определим его код — `11`.
Печатаем эти цифры.

После кодирвоания всей строки `ababcaca`, программа должна напечатать `010010110110`.
