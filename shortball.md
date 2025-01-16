# Шортбол

Шортбол — игра для конференций. Позволяет немного размяться, а также прокачать знание деталей в какой-то технической области.
Для игры нужен мяч, который можно безопасно перебрасывать друг другу.
Такой, который ничего не разобьёт и никому ничего не сломает.

Участникам можно выдавать бейджи с ролями, но это не обязательно, потому что участников немного и они быстро друг друга запоминают.

<p align="center">
  <img src="https://github.com/user-attachments/assets/c2363c8c-2a64-4bfa-aed2-c7d1529b30d2" width="60%" alt="Шортбол" />
</p>

Название *шортбол* выбрано потому, что игра обычно короткая, минут на десять.

Для игры нужна тема, например, *Как работает интернет*.
Раздаём участникам роли, например, *Браузер*, *DNS-сервер*, *Файлвол*, *Реверс-прокси*, *HTTP-сервер*.
Состав можно варьировать в зависимости от количества участников.
Ведущий может отвечать за второстепенные роли, которые возникают не всех сценариях.
Каждый раунд начинаеся с игрока-инициатора, в нашем случае это *Браузер*.
Игрок называет своё имя и роль, говорит, что он собирается сделать и кидает мяч игроку, который должен быть следующим в цепочке.
В нашем случае это должен быть *DNS-сервер*.
Если *Браузер* бросил мяч неправильно, разбираем, почему это неправильно.
В идеале, знаниями должны делиться сами участники, но Ведущий может помогать.

Также Ведущий может подсказывать игроками разные варианты развития событий.
Например, ход получает игрок *Кэш*.
Возможные варианты: а) в кэше есть нужное значение; б) в кэше нет нужного значения.
Во втором случае реализуется развёрнутый вариант сценария — получить значение из базы и сохранить его в кэше.
Благодаря вариантам, можно проводить несколько раундов игры с одними и теми же участниками.

Ниже представлены сценарии для игры в виде даиграмм последовательностей.

## Микросервисная архитектура

Варианты сценария:
* Просрочен `access_token`, не просрочен `refresh_token`.
* Просрочены и `access_token` и `refresh_token`.
* Запись не найдена в Postgres.

```mermaid
sequenceDiagram
  participant Frontend
  participant API Gateway
  participant Microservice
  participant Redis
  participant Postgres
  Frontend->>API Gateway: GET /items/314
  API Gateway->>Microservice: GET /items/314
  Microservice->>Redis: GET 314
  Redis->>Microservice: (nil)
  Microservice->>Postgres: SELECT id, value FROM items WHERE id = 314
  Postgres->>Microservice: (314, "Hello, world!")
  Microservice->>Redis: SET 314 "Hello, worlds!"
  Microservice->>API Gateway: { id: 314, value: "Hello, world!" }
  API Gateway->>Frontend: { id: 314, value: "Hello, world!" }
```

## SMTP

```mermaid
sequenceDiagram
  participant SMTP Sender
  participant DNS
  participant SMTP Reciever
  SMTP Sender->>DNS: MX_Resource_Record_Question = gmail.com
  DNS->>SMTP Sender: MX_Record_Record_Answer = smtp.gmail.com,<br />A_Resource_Record = 108.177.15.108
  SMTP Sender->>SMTP Reciever: SYN,<br />,TCP Port = 25
  SMTP Reciever->>SMTP Sender: SYN_ACK
  SMTP Sender->>SMTP Reciever: ACK
  SMTP Reciever->>SMTP Sender: 220 smtp.gmail.com SMTP Ready\r\n
  SMTP Sender->>SMTP Reciever: HELO smtp.mail.ru\r\n
  SMTP Reciever->>SMTP Sender: ACK
  SMTP Reciever->>DNS: Pointer Query<br />217.69.139.160
  DNS->>SMTP Reciever: Pointer Reply: smtp.mail.ru
  SMTP Reciever->>SMTP Sender: 250 smpt.gmail.com \r\n
  SMTP Sender->>SMTP Reciever: VRFY john.silver jim hawkins\r\n
  SMTP Reciever->>SMTP Sender: 550 Unknown user: john.silver\r\n
  SMTP Sender->>SMTP Reciever: VRFY jim.hawkins\r\n
  SMTP Reciever->>SMTP Sender: 250 OK\r\n
  SMTP Sender->>SMTP Reciever: VRFY ben.gunn\r\n
  SMTP Reciever->>SMTP Sender: 250 OK\r\n
  SMTP Sender->>SMTP Reciever: MAIL FROM:captain.smollett@mail.ru\r\n
  SMTP Reciever->>SMTP Sender: 250 OK\r\n
  SMTP Sender->>SMTP Reciever: RCPT TO:john.silver@gmail.com\r\n
  SMTP Reciever->>SMTP Sender: 250 OK\r\n
  SMTP Sender->>SMTP Reciever: RCPT TO:jim.hawkins@gmail.com\r\n
  SMTP Reciever->>SMTP Sender: 250 OK\r\n
  SMTP Sender->>SMTP Reciever: DATA\r\n
  SMTP Reciever->>SMTP Sender: 354 Enter e-mail, end with .
  SMTP Sender->>SMTP Reciever: *Заголовки, пустая строка, текст письма*\r\n
  SMTP Reciever->>SMTP Sender: ACK
  SMTP Sender->>SMTP Reciever: .\r\n
  SMTP Reciever->>SMTP Sender: 250 OK Mail accepted\r\n
  SMTP Sender->>SMTP Reciever: QUIT
  SMTP Reciever->>SMTP Sender: FIN
  SMTP Sender->>SMTP Reciever: ACK
  SMTP Sender->>SMTP Reciever: FIN
  SMTP Reciever->>SMTP Sender: ACK
```
