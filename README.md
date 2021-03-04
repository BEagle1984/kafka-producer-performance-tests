```
100'000 messages of 50 bytes

Confluent.Kafka Produce w/ callback                                         : 509'241.46 msg/s | 100'000 in 0.196s
Confluent.Kafka Produce w/ callback, LingerMs=100                           : 472'845.21 msg/s | 100'000 in 0.211s ->     +7.70%
Confluent.Kafka Produce w/ callback, LingerMs=100, BatchSize=50'000'000     : 495'467.22 msg/s | 100'000 in 0.202s ->     +2.78%
Silverback ProduceAsync w/ callbacks                                        :  78'871.34 msg/s | 100'000 in 1.268s ->   +545.66%
Silverback ProduceAsync w/ callbacks, LingerMs=100                          :  88'743.22 msg/s | 100'000 in 1.127s ->   +473.84%
Silverback ProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000    :  85'335.26 msg/s | 100'000 in 1.172s ->   +496.75%
Silverback RawProduceAsync w/ callbacks                                     : 411'613.60 msg/s | 100'000 in 0.243s ->    +23.72%
Silverback RawProduceAsync w/ callbacks, LingerMs=100                       : 307'803.21 msg/s | 100'000 in 0.325s ->    +65.44%
Silverback RawProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000 : 325'241.12 msg/s | 100'000 in 0.307s ->    +56.57%

100'000 messages of 100 bytes

Confluent.Kafka Produce w/ callback                                         : 555'625.93 msg/s | 100'000 in 0.180s
Confluent.Kafka Produce w/ callback, LingerMs=100                           : 381'410.51 msg/s | 100'000 in 0.262s ->    +45.68%
Confluent.Kafka Produce w/ callback, LingerMs=100, BatchSize=50'000'000     : 361'405.51 msg/s | 100'000 in 0.277s ->    +53.74%
Silverback ProduceAsync w/ callbacks                                        :  85'642.14 msg/s | 100'000 in 1.168s ->   +548.78%
Silverback ProduceAsync w/ callbacks, LingerMs=100                          :  85'809.52 msg/s | 100'000 in 1.165s ->   +547.51%
Silverback ProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000    :  82'026.03 msg/s | 100'000 in 1.219s ->   +577.38%
Silverback RawProduceAsync w/ callbacks                                     : 390'744.05 msg/s | 100'000 in 0.256s ->    +42.20%
Silverback RawProduceAsync w/ callbacks, LingerMs=100                       : 277'526.85 msg/s | 100'000 in 0.360s ->   +100.21%
Silverback RawProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000 : 257'837.02 msg/s | 100'000 in 0.388s ->   +115.50%

100'000 messages of 1'000 bytes

Confluent.Kafka Produce w/ callback                                         :  77'345.02 msg/s | 100'000 in 1.293s
Confluent.Kafka Produce w/ callback, LingerMs=100                           :  76'594.28 msg/s | 100'000 in 1.306s ->     +0.98%
Confluent.Kafka Produce w/ callback, LingerMs=100, BatchSize=50'000'000     :  71'647.57 msg/s | 100'000 in 1.396s ->     +7.95%
Silverback ProduceAsync w/ callbacks                                        :  64'270.91 msg/s | 100'000 in 1.556s ->    +20.34%
Silverback ProduceAsync w/ callbacks, LingerMs=100                          :  60'650.50 msg/s | 100'000 in 1.649s ->    +27.53%
Silverback ProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000    :  56'211.77 msg/s | 100'000 in 1.779s ->    +37.60%
Silverback RawProduceAsync w/ callbacks                                     :  67'439.68 msg/s | 100'000 in 1.483s ->    +14.69%
Silverback RawProduceAsync w/ callbacks, LingerMs=100                       :  60'503.24 msg/s | 100'000 in 1.653s ->    +27.84%
Silverback RawProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000 :  57'989.27 msg/s | 100'000 in 1.724s ->    +33.38%

100'000 messages of 10'000 bytes

Confluent.Kafka Produce w/ callback                                         :   7'401.22 msg/s | 100'000 in 13.511s ->     +2.72%
Confluent.Kafka Produce w/ callback, LingerMs=100                           :   7'602.31 msg/s | 100'000 in 13.154s
Confluent.Kafka Produce w/ callback, LingerMs=100, BatchSize=50'000'000     :   7'470.19 msg/s | 100'000 in 13.387s ->     +1.77%
Silverback ProduceAsync w/ callbacks                                        :   7'323.51 msg/s | 100'000 in 13.655s ->     +3.81%
Silverback ProduceAsync w/ callbacks, LingerMs=100                          :   7'185.20 msg/s | 100'000 in 13.918s ->     +5.81%
Silverback ProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000    :   7'166.93 msg/s | 100'000 in 13.953s ->     +6.07%
Silverback RawProduceAsync w/ callbacks                                     :   7'562.22 msg/s | 100'000 in 13.224s ->     +0.53%
Silverback RawProduceAsync w/ callbacks, LingerMs=100                       :   7'523.17 msg/s | 100'000 in 13.292s ->     +1.05%
Silverback RawProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000 :   7'490.53 msg/s | 100'000 in 13.350s ->     +1.49%
```