# 100'000 messages in different sizes

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

# Different message sizes with constant total bytes

```
10'000'000 messages of 50 bytes

Confluent.Kafka Produce w/ callback                                         : 738'541.27 msg/s | 10'000'000 in 13.540s
Confluent.Kafka Produce w/ callback, LingerMs=100                           : 624'606.83 msg/s | 10'000'000 in 16.010s ->    +18.24%
Confluent.Kafka Produce w/ callback, LingerMs=100, BatchSize=50'000'000     : 584'688.54 msg/s | 10'000'000 in 17.103s ->    +26.31%
Silverback ProduceAsync w/ callbacks                                        :  84'247.13 msg/s | 10'000'000 in 118.698s ->   +776.64%
Silverback ProduceAsync w/ callbacks, LingerMs=100                          :  94'458.01 msg/s | 10'000'000 in 105.867s ->   +681.87%
Silverback ProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000    :  94'241.01 msg/s | 10'000'000 in 106.111s ->   +683.67%
Silverback RawProduceAsync w/ callbacks                                     : 378'660.89 msg/s | 10'000'000 in 26.409s ->    +95.04%
Silverback RawProduceAsync w/ callbacks, LingerMs=100                       : 387'835.85 msg/s | 10'000'000 in 25.784s ->    +90.43%
Silverback RawProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000 : 391'558.29 msg/s | 10'000'000 in 25.539s ->    +88.62%

5'000'000 messages of 100 bytes

Confluent.Kafka Produce w/ callback                                         : 578'768.59 msg/s | 5'000'000 in 8.639s
Confluent.Kafka Produce w/ callback, LingerMs=100                           : 435'290.12 msg/s | 5'000'000 in 11.487s ->    +32.96%
Confluent.Kafka Produce w/ callback, LingerMs=100, BatchSize=50'000'000     : 537'487.22 msg/s | 5'000'000 in 9.303s ->     +7.68%
Silverback ProduceAsync w/ callbacks                                        :  80'960.46 msg/s | 5'000'000 in 61.759s ->   +614.88%
Silverback ProduceAsync w/ callbacks, LingerMs=100                          :  89'772.65 msg/s | 5'000'000 in 55.696s ->   +544.70%
Silverback ProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000    :  85'470.57 msg/s | 5'000'000 in 58.500s ->   +577.16%
Silverback RawProduceAsync w/ callbacks                                     : 320'803.73 msg/s | 5'000'000 in 15.586s ->    +80.41%
Silverback RawProduceAsync w/ callbacks, LingerMs=100                       : 273'679.82 msg/s | 5'000'000 in 18.270s ->   +111.48%
Silverback RawProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000 : 352'528.92 msg/s | 5'000'000 in 14.183s ->    +64.18%

500'000 messages of 1'000 bytes

Confluent.Kafka Produce w/ callback                                         :  66'617.39 msg/s | 500'000 in 7.506s ->     +7.05%
Confluent.Kafka Produce w/ callback, LingerMs=100                           :  64'528.56 msg/s | 500'000 in 7.749s ->    +10.51%
Confluent.Kafka Produce w/ callback, LingerMs=100, BatchSize=50'000'000     :  70'656.64 msg/s | 500'000 in 7.076s ->     +0.93%
Silverback ProduceAsync w/ callbacks                                        :  59'553.22 msg/s | 500'000 in 8.396s ->    +19.74%
Silverback ProduceAsync w/ callbacks, LingerMs=100                          :  60'549.59 msg/s | 500'000 in 8.258s ->    +17.77%
Silverback ProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000    :  56'662.37 msg/s | 500'000 in 8.824s ->    +25.85%
Silverback RawProduceAsync w/ callbacks                                     :  71'310.85 msg/s | 500'000 in 7.012s
Silverback RawProduceAsync w/ callbacks, LingerMs=100                       :  69'317.94 msg/s | 500'000 in 7.213s ->     +2.88%
Silverback RawProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000 :  67'911.26 msg/s | 500'000 in 7.363s ->     +5.01%

50'000 messages of 10'000 bytes

Confluent.Kafka Produce w/ callback                                         :   7'438.33 msg/s | 50'000 in 6.722s ->     +4.41%
Confluent.Kafka Produce w/ callback, LingerMs=100                           :   7'766.65 msg/s | 50'000 in 6.438s
Confluent.Kafka Produce w/ callback, LingerMs=100, BatchSize=50'000'000     :   7'361.50 msg/s | 50'000 in 6.792s ->     +5.50%
Silverback ProduceAsync w/ callbacks                                        :   7'529.94 msg/s | 50'000 in 6.640s ->     +3.14%
Silverback ProduceAsync w/ callbacks, LingerMs=100                          :   7'338.65 msg/s | 50'000 in 6.813s ->     +5.83%
Silverback ProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000    :   7'157.05 msg/s | 50'000 in 6.986s ->     +8.52%
Silverback RawProduceAsync w/ callbacks                                     :   7'536.79 msg/s | 50'000 in 6.634s ->     +3.05%
Silverback RawProduceAsync w/ callbacks, LingerMs=100                       :   7'642.73 msg/s | 50'000 in 6.542s ->     +1.62%
Silverback RawProduceAsync w/ callbacks, LingerMs=100, BatchSize=50'000'000 :   7'494.83 msg/s | 50'000 in 6.671s ->     +3.63%
```
