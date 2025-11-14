# Импорт/экспорт

При локальной разработке бывает нужно сделать импорт или экспорт кейклока. Это делается так:

## 1. Остановить кейклок
```shell
docker compose down keycloak
```

## 2. Сделать экспорт/импорт

### Импорт:
```shell
docker run --rm --env-file .\keycloak\.env -v .\keycloak\export:/export quay.io/keycloak/keycloak:26.4 import --file /export/kc.json
```
Если это до этого импорта вы не запускали кейклок то нужно создать админского пользователя:
```shell
docker run --rm --env-file .\keycloak\.env quay.io/keycloak/keycloak:26.4 bootstrap-admin user --no-prompt --username:env KC_BOOTSTRAP_ADMIN_USERNAME --password:env KC_BOOTSTRAP_ADMIN_PASSWORD
```


### Экспорт:
```shell
docker run --rm --env-file .\keycloak\.env -v .\keycloak\export:/export quay.io/keycloak/keycloak:26.4 export --file /export/kc.json --realm koworking-dev
```

## 3. Поднять кейклок заново
```shell
docker compose up keycloak
```