# 在 Docker 中安装 Kafka

1. `kafka` 需要 `zookeeper` 管理，所以需要先安装 `zookeeper` ，通过如下命令安装

```powershell
docker pull wurstmeister/zookeeper
```

2. 启动 `zookeeper` 镜像生产容器

```powershell
docker run -d --name zookeeper -p 2181:2181 -v /etc/localtime:/etc/localtime wurstmeister/zookeeper
```

3. 下载 `kafka` 镜像

```powershell
docker pull wurstmeister/kafka
```

4. 启动 `kafka` 镜像生成容器

```powershell
docker run -d --name kafka -p 9092:9092 -e KAFKA_BROKER_ID=0 -e KAFKA_ZOOKEEPER_CONNECT=172.23.81.147:2181/kafka -e KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://172.23.81.147:9092 -e KAFKA_LISTENERS=PLAINTEXT://0.0.0.0:9092 -v /etc/localtime:/etc/localtime wurstmeister/kafka 
```

**注意**

1. `-e KAFKA_BROKER_ID=0` 在 `kafka` 集群中，每个 `kafka` 都有一个 `BROKER_ID` 来区分自己

2. `-e KAFKA_ZOOKEEPER_CONNECT=192.168.155.56:2181/kafka` 配置 `zookeeper` 管理 `kafka` 的路径 `192.168.155.56:2181/kafka`

3. `-e KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://192.168.155.56:9092` 把 `kafka` 的地址端口注册给 `zookeeper`

4. `-e KAFKA_LISTENERS=PLAINTEXT://0.0.0.0:9092` 配置 `kafka` 的监听端口

5. `-v /etc/localtime:/etc/localtime` 容器时间同步虚拟机的时间

6. 验证 `Kafka` 是否可用
   
   1. 进入 `Docker kafka` 实例控制台
   
   2. 进入 `opt/kafka_version/bin` 目录下，执行发送消息命令
      
      ```powershell
      ./kafka-console-producer.sh --broker-list localhost:9092 --topic jeremy
      ```
   
   3. 输入要发送的消息
      
      ```powershell
      > Hey Jeremy
      ```
   
   4. 进入 `Docker kafka` 实例控制台，进入 `opt/kafka_version/bin` 目录下，执行j接收消息命令
      
      ```powershell
      kafka-console-consumer.sh --bootstrap-server localhost:9092 --topic jeremy --from-beginning
      ```


