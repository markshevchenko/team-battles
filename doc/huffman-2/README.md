﻿# Алгоритм Хаффмана (декодирование)

Реализовать декодирование текста по алгоритму Хаффмана.

Алгоритм Хаффмана состоит из двух этапов: построение дерева Хаффмана и декодирование.
В этой задаче дерево уже построено, так что вам нужно реализовать только декодирование.

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

После `N` строк, содержащих команды, следует последняя строка, в которой находится код для декодироваания, сотоящий из символов `0` и `1`.

**Пример входа программы**
```text
5
Pa
Pb
Pc
C
C
01001011011
```

Эти команды строят дерево, показанное на рисунке.

## Декодирование

Прежде, чем приступить к декодированию, пометим каждую левую ветвь дерева Хаффмана цифрой `0`, а каждую правую — цифрой `1` (на рисунке это уже сделано, так что вы можете с ним сверяться).

Для перемещения по дереву Хаффмана, нам потребуется указатель, который в самом начале работы должен указывать на корень.

Читаем из входной строки первую цифру, в нашем случае `0`.
Цифре `0` соответствует левое поддерево, поэтому перемещаем указатель так, чтобы он указавал на левое поддерево.
Мы добрались до листа `a`: печатаем символ `a` и возвращаем указатель обратно на корень дерева.

Читаем из входной строки следующую цифру, это `1`.
Цифре `1` соответствует правое поддерево, поэтому перемещаем указатель так, чтобы он указавал на правое поддерево.
Так как текущий узел — это ветвление, продолжаем поиск.

Читаем из входной строки следующую цифру, это `0`.
Цифре `0` соответствует левое поддерево, поэтому перемещаем указатель так, чтобы он указавал на левое поддерево.
Мы добрались до листа `b`: печатаем символ `b` и возвращаем указатель обратно на корень дерева.

Прочитав код `010010110110`, программа должна напечатать текст `ababcaca`.
