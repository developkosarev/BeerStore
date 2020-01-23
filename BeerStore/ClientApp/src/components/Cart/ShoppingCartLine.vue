<template>

    <div class="card">
        <div class="card-body p-2">
            <div class="row no-gutters">

                <div class="col-10 col-md-6 text-left">
                    {{ line.product.descr }}
                </div>

                <div class="col-2 col-md-1 order-md-2">
                    <button type="button" class="btn btn-outline-danger btn-sm"
                            v-on:click="sendRemoveEvent"
                    >
                        <i class="fa fa-trash" aria-hidden="true"></i>
                    </button>
                </div>

                <div class="col-12 col-md-5 order-md-1">
                    <div class="form-row pt-1 pt-md-0">
                        <div class="col-2 pl-0 pr-0">
                            <strong>{{ line.price }}
                                <span class="text-muted m-0">x</span>
                            </strong>
                        </div>
                        <div class="col-2 pl-0 pr-0">
                            <!--
                            <input type="number" class="form-control input-sm"
                                   v-bind:value="qvalue"
                                   v-on:input="sendChangeEvent"
                            />
                            -->

                            <b-form-input size="sm"
                                          type="number"
                                          placeholder="Количество"
                                          v-model="qvalue"
                                          v-on:input="sendChangeEvent"
                                          v-on:click.native="onClick"
                                          class="pl-1 pr-1"
                            >

                            </b-form-input>

                        </div>
                        <div class="col-3 pl-0 pr-0">
                            <strong>
                                <span class="text-muted m-0">=</span>
                                {{ (line.quantity * line.price).toFixed(2) }}
                            </strong>
                        </div>

                        <div class="col-5">
                            <template v-if="!line.marketingEventLoad">
                                <button type="button" class="btn btn-outline-primary btn-sm"
                                        v-on:click="sendGetMarketingEvent"
                                >
                                    <i class="fa fa-percent" aria-hidden="true"></i>
                                    Маркетинг
                                </button>
                            </template>
                            <template v-else>
                                <b-form-select v-model="marketingEventValue" :options="line.marketingEventItems" size="sm"
                                               v-on:input="sendMarketingEvent"
                                />
                            </template>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>



    <!--
    <tr>
        <td>

            <button class="btn btn-warning btn-sm"
            >
                -уп
            </button>

            <input type="number" class="form-control-sm m-1"
                style="width:5em"
                v-bind:value="qvalue"
                v-on:input="sendChangeEvent"/>

            <button class="btn btn-success btn-sm"
            >
                +уп
            </button>

        </td>
        <td>
            {{ line.product.descr }}
        </td>
        <td class="text-right">
            {{ line.price }}
        </td>
        <td class="text-right">
            {{ (line.quantity * line.price) }}
        </td>
        <td class="text-center">
            <button class="btn btn-sm btn-danger"
                    v-on:click="sendRemoveEvent">
                Удалить
            </button>
        </td>
    </tr>
    -->

</template>

<script>
    export default {
        props: ["line"],
        data: function() {
            return {
                qvalue: this.line.quantity,
                marketingEventValue: this.line.marketingEvent,
                marketingEventLoad: this.line.marketingEventLoad,
                marketingEventItems: this.line.marketingEventItems
            }
        },
        methods: {
            //sendChangeEvent($event)
            sendChangeEvent() {
              if (Number(this.qvalue) >= 0) {
                this.$emit("quantity", Number(this.qvalue));
              } else {
                this.$emit("quantity", 1);
                this.qvalue = 1;
              }

                //if ($event.target.value > 0) {
                    //this.$emit("quantity", Number($event.target.value));
                    //this.qvalue = $event.target.value;
                // } else {
                //     this.$emit("quantity", 1);
                //     this.qvalue = 1;
                //     $event.target.value = this.qvalue;
                // }
            },
            onClick(event){
              event.target.select();
            },
            sendRemoveEvent() {
                this.$emit("remove", this.line);
            },
            sendGetMarketingEvent() {
              this.$emit("getMarketingEvent", this.line);
            },
            sendMarketingEvent() {
              this.$emit("marketingEvent", this.marketingEventValue);
            }
        }
    }
</script>
