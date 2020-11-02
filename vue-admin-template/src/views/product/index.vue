<template>
  <div class="app-container">
    <div class="filter-container">
      <el-collapse accordion v-model="activeName">
        <el-collapse-item title="查询条件" name="1">
          <el-form :inline="true" :model="filterModel">
            <el-form-item label="配置名称">
              <el-input
                v-model="filterModel.value.value"
                placeholder="配置名称"
                class="filter-item"
                @keyup.enter.native="handleFilter"
              />
            </el-form-item>
            <el-form-item label="添加时间">
              <el-date-picker
                v-model="filterModel.createTime.value"
                :picker-options="daterangeOptions"
                value-format="yyyy-MM-dd"
                range-separator="至"
                start-placeholder="开始时间"
                end-placeholder="结束时间"
                style="width: 250px;"
                type="daterange"
                placement="bottom-end"
                @keyup.enter.native="handleFilter"
              />
            </el-form-item>
          </el-form>
        </el-collapse-item>
      </el-collapse>
    </div>
    <el-row class="el-table-header-buttom">
      <el-button
        v-waves
        class="filter-item"
        type="primary"
        icon="el-icon-search"
        @click="handleFilter"
      >查询</el-button>

      <el-button
        v-waves
        class="filter-item"
        type="primary"
        icon="el-icon-search"
        @click="handleCreate"
      >新增</el-button>
    </el-row>
    <el-table
      :key="tableKey"
      v-loading="listLoading"
      :data="list"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      @sort-change="sortChange"
    >
      <el-table-column label="产品编码" prop="productCode" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.productCode }}</span>
        </template>
      </el-table-column>
      <el-table-column label="产品名称" prop="productName" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.productName }}</span>
        </template>
      </el-table-column>

      <el-table-column label="库存量" prop="stockNum" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.stockNum }}</span>
        </template>
      </el-table-column>
      <el-table-column label="限购量" prop="limitedNum" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.limitedNum }}</span>
        </template>
      </el-table-column>

      <el-table-column label="价格" prop="price" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.price}}</span>
        </template>
      </el-table-column>

       <el-table-column label="颜色" prop="color" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.color}}</span>
        </template>
      </el-table-column>

       <el-table-column label="品牌" prop="brand" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.brand}}</span>
        </template>
      </el-table-column>

      <el-table-column label="开团日期" prop="openingTime" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.openingTime}}</span>
        </template>
      </el-table-column>

      <el-table-column label="图片地址" prop="image" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.image}}</span>
        </template>
      </el-table-column>

      <el-table-column label="描述">
        <template slot-scope="{row}">
          <span>{{ row.des }}</span>
        </template>
      </el-table-column>

      <el-table-column label="操作" width="250" class-name="small-padding fixed-width">
        <template slot-scope="{row,$index}">
          <el-button type="primary" size="mini" @click="handleUpdate(row)">修改</el-button>
          <el-button size="mini" type="danger" @click="handleDelete(row,$index)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination
      v-show="total>0"
      :total="total"
      :page.sync="listQuery.page"
      :limit.sync="listQuery.limit"
      @pagination="getList"
    />

    <el-dialog
      v-el-drag-dialog
      :title="dialogTitle"
      :visible.sync="dialogFormVisible"
      width="40%"
      top="2vh"
    >
      <el-form ref="dataForm" :model="temp" label-position="right" label-width="100px">
        <el-row>
          <el-col :span="24">
            <el-form-item label="产品编码" prop="productCode" :rules="rules.checkNull">
              <el-input v-model="temp.productCode" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <el-form-item label="产品名称" prop="productName" :rules="rules.checkNull">
              <el-input v-model="temp.productName" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <el-form-item label="库存量" prop="stockNum" :rules="rules.checkNull">
              <el-input v-model="temp.stockNum" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <el-form-item label="限购量" prop="limitedNum" :rules="rules.checkNull">
              <el-input v-model="temp.limitedNum" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="价格" prop="price" :rules="rules.checkNull">
              <el-input v-model="temp.price" />
            </el-form-item>
          </el-col>
        </el-row>
      <el-row>
          <el-col :span="24">
            <el-form-item label="颜色" prop="color" :rules="rules.checkNull">
              <el-input v-model="temp.color" />
            </el-form-item>
          </el-col>
        </el-row>
  <el-row>
          <el-col :span="24">
            <el-form-item label="品牌" prop="brand" :rules="rules.checkNull">
              <el-input v-model="temp.brand" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="开团日期" prop="openingTime" :rules="rules.checkNull">
              <el-date-picker
                v-model="temp.openingTime"
                type="datetime"
                value-format="yyyy-MM-dd HH:mm:ss"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <el-form-item label="图片地址" prop="image" :rules="rules.checkNull">
              <el-input v-model="temp.image" />
            </el-form-item>
          </el-col>
        </el-row>

          <el-row>
          <el-col :span="24">
            <el-form-item label="图片详情地址" prop="imageDetail" :rules="rules.checkNull">
              <el-input v-model="temp.imageDetail" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <el-form-item label="描述" prop="des">
              <el-input v-model="temp.des" type="textarea" :autosize="{minRows: 2,maxRows: 5}" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>

      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">关闭</el-button>
        <el-button type="primary" @click="dialogStatus==='create'?createData():updateData()">保存</el-button>
      </div>
    </el-dialog>
  </div>
</template>   
<script>
import waves from "@/directive/waves"; // waves directive
import Pagination from "@/components/Pagination";
import myAction from "@/utils/baseutil";
import { getPageList, addOrEdit, del } from "@/api/product";
export default {
  name: "index",
  directives: { waves },
  components: { Pagination },
  filters: {
    formatTime: function(val) {
      return myAction.formatTime(val);
    }
  },
  data() {
    let currentData = {
      filterModel: {
        value: {
          field: "ProductName",
          method: "Contains",
          value: "",
          prefix: "",
          operator: "And"
        },
        createTime: {
          field: "OpeningTime",
          method: "Between",
          value: "",
          prefix: "",
          operator: "And"
        }
      },
      temp: {
        id: "",
        productCode: "",
        productName: "",
        stockNum: 0,
        limitedNum: 0,
        price: 0,
        des: "",
        image: "",
        imageDetail:'',
        color:'',
        brand:'',
        openingTime: ""
      },
      orderArr: []
    };
    let custRules = {};
    var data = $.extend(false, myAction.setBaseVueData, currentData);
    data.rules = $.extend(false, myAction.setBaseValidateRules(), custRules);
    return data;
  },
  created() {
    this.getList();
  },
  methods: {
    getList() {
      this.listLoading = true;
      let param = myAction.getQueryModel(
        this.listQuery.limit,
        this.listQuery.page,
        this.total,
        this.filterModel,
        this.orderArr,
        false
      );
      getPageList(param).then(response => {
        this.list = response.pageData;
        this.total = response.total;
        // Just to simulate the time of the request
        setTimeout(() => {
          this.listLoading = false;
        }, 1.5 * 300);
      });
    },
    //查询处理的事件
    handleFilter() {
      this.listQuery.page = 1;
      this.getList();
    },
    //排序事件
    sortChange(data) {
      this.orderArr = [];
      this.orderArr.push(data);
      this.handleFilter();
    },
    resetTemp() {
      this.temp = {
        id: "00000000-0000-0000-0000-000000000000",
        productCode: "",
        productName: "",
        stockNum: 0,
        limitedNum: 0,
        price: 0,
        des: "",
        image: "",
        imageDetail:'',
        color:'',
        brand:'',
        openingTime: ""
      };
    },
    //新增
    handleCreate() {
      this.resetTemp();
      this.dialogStatus = "create";
      this.dialogTitle = "新增产品";
      this.dialogFormVisible = true;
      this.$nextTick(() => {
        this.$refs["dataForm"].clearValidate();
      });
    },
    //提交数据
    createData() {
      this.$refs["dataForm"].validate(valid => {
        if (valid) {
          var data = this.temp;
          //  console.log(data)
          addOrEdit(data).then(response => {
            this.temp.id = response.data;
            this.list.unshift(this.temp);
            this.total++;
            this.dialogFormVisible = false;
            myAction.getMessFunc(response, this);
          });
        }
      });
    },
    handleDelete(row, index) {
      var title = '<span style="color: red;">是否要删除这条数据?</span>';
      this.$confirm(title, "提示", {
        dangerouslyUseHTMLString: true,
        type: "warning",
        confirmButtonText: "确定",
        cancelButtonText: "取消"
      })
        .then(() => {
          let data = '"' + row.id + '"';
          del(data).then(response => {
            this.list.splice(index, 1);
            this.total--;
            myAction.getMessFunc(response, this);
          });
        })
        .catch(action => {});
    },
    handleUpdate(row) {
      this.temp = Object.assign({}, row); // copy obj
      this.dialogStatus = "update";
      this.dialogTitle = "修改商品";
      this.dialogFormVisible = true;
      this.$nextTick(() => {
        this.$refs["dataForm"].clearValidate();
      });
    },
    updateData() {
      this.$refs["dataForm"].validate(valid => {
        if (valid) {
          const tempData = Object.assign({}, this.temp);
          var data = tempData;
          addOrEdit(data).then(response => {
            const index = this.list.findIndex(v => v.id === this.temp.id);
            this.list.splice(index, 1, data);
            this.dialogFormVisible = false;
            myAction.getMessFunc(response, this);
          });
        }
      });
    }
  },
  mounted() {}
};
</script>

