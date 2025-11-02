# Koworking
Сайт для поиска постоянной или временной работы

# TODO:
- [x] utm-ссылки + clickhouse
- [ ] сокращённые ссылки на приложение
- [x] аутентификация
- [ ] создание вакансий
- [ ] отклик на вакансии
- [ ] чат в приложении

# Сборка образов

## web
```shell
docker build -t un1ver5e/koworking-web --push --target web --build-arg VITE_KEYCLOAK_URL=https://kckwr.un1ver5e.ru/realms/koworking .
```

## api
```shell
docker build -t un1ver5e/koworking-api -f .\api\Dockerfile --push .
```
