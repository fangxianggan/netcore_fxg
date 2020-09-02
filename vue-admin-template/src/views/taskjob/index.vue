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
      <el-table-column label="任务组" prop="taskGroup" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.taskGroup }}</span>
        </template>
      </el-table-column>
      <el-table-column label="任务名称" prop="taskName" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.taskName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="Cron表达式" prop="cronExpression" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.cronExpression }}</span>
        </template>
      </el-table-column>
      <el-table-column label="任务描述" prop="description" sortable="custom" width="200px">
        <template slot-scope="{row}">
          <span>{{ row.description }}</span>
        </template>
      </el-table-column>

      <el-table-column label="接口地址" prop="apiUrl" sortable="custom">
        <template slot-scope="{row}">
          <span>{{ row.apiUrl}}</span>
        </template>
      </el-table-column>
      <el-table-column label="开始执行时间">
        <template slot-scope="{row}">
          <span>{{ row.startRunTime|formatTime}}</span>
        </template>
      </el-table-column>
      <el-table-column label="结束时间">
        <template slot-scope="{row}">
          <span>{{ row.endRunTime|formatTime}}</span>
        </template>
      </el-table-column>
      <el-table-column label="运行次数">
        <template slot-scope="{row}">
          <span>{{ row.runCount }}</span>
        </template>
      </el-table-column>
      <el-table-column label="任务状态">
        <template slot-scope="{row}">
          <span>{{ row.taskState }}</span>
        </template>
      </el-table-column>

      <el-table-column label="操作" width="250" class-name="small-padding fixed-width">
        <template slot-scope="{row,$index}">
          <el-button
            type="success"
            v-if="row.fileCategory!='document'"
            size="mini"
            @click="handleStart(row)"
          >启动</el-button>
          <el-button type="primary" size="mini" @click="handleStop(row)">暂停</el-button>

          <el-button
            v-if="row.status!='deleted'"
            size="mini"
            type="danger"
            @click="handleDelete(row,$index)"
          >删除</el-button>
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
            <el-form-item label="任务组名" prop="taskGroup" :rules="rules.checkNull">
              <el-input v-model="temp.taskGroup" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <el-form-item label="任务名称" prop="taskName" :rules="rules.checkNull">
              <el-input v-model="temp.taskName" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <el-form-item label="请求地址" prop="apiUrl" :rules="rules.checkNull">
              <el-input v-model="temp.apiUrl" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="开始时间" prop="startRunTime" :rules="rules.checkNull">
              <el-date-picker v-model="temp.startRunTime" type="datetime" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="结束时间" prop="endRunTime">
              <el-date-picker v-model="temp.endRunTime" type="datetime" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="Corn表达式" prop="cronExpression" :rules="rules.checkNull">
              <el-popover v-model="cronPopover">
                <cron @change="changeCron" @close="cronPopover=false" i18n="cn"></cron>
                <el-input
                  slot="reference"
                  @click="cronPopover=true"
                  v-model="temp.cronExpression"
                  placeholder="请输入定时策略"
                ></el-input>
              </el-popover>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="请求类型" prop="requestType" :rules="rules.checkNull">
              <el-radio v-model="temp.requestType" label="Get">Get</el-radio>
              <el-radio v-model="temp.requestType" label="Post">Post</el-radio>
              <el-radio v-model="temp.requestType" label="Put">Put</el-radio>
              <el-radio v-model="temp.requestType" label="Delete">Delete</el-radio>
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <el-form-item label="请求头" prop="requestHead">
              <el-input v-model="temp.requestHead" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <el-form-item label="请求参数" prop="requestParams">
              <el-input
                v-model="temp.requestParams"
                type="textarea"
                :autosize="{minRows: 2,maxRows: 5}"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="24">
            <el-form-item label="任务描述" prop="description">
              <el-input
                v-model="temp.description"
                type="textarea"
                :autosize="{minRows: 2,maxRows: 5}"
              />
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
import { cron } from "vue-cron";
import waves from "@/directive/waves"; // waves directive
import Pagination from "@/components/Pagination";
import {
  getPageList,
  addOrEdit,
  addJob,
  stopJob,
  resumeJob,
  deleteJob
} from "@/api/taskjob";
import myAction from "@/utils/baseutil";
import qs from 'qs'
export default {
  name: "upload",
  directives: { waves },
  components: { Pagination, cron },
  filters: {
    formatTime: function(val) {
      return myAction.formatTime(val);
    }
  },
  data() {
    let currentData = {
      filterModel: {
        value: {
          field: "fileName",
          method: "Contains",
          value: "",
          prefix: "",
          operator: "And"
        },
        createTime: {
          field: "UploadTime",
          method: "Between",
          value: "",
          prefix: "",
          operator: "And"
        }
      },
      temp: {
        id: "",
        taskGroup: "",
        taskName: "",
        description: "",
        apiUrl: "",
        requestType: "",
        requestHead: "",
        requestParams: "",
        exceptionMsg: "",
        cronExpression: "",
        cronExpressionDescription: "",
        startRunTime: "",
        endRunTime: "",
        runCount: 0,
        taskState: 0
      },
      orderArr: [],
      cronPopover: false,
      cron: ""
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
    changeCron(val) {
      this.temp.cronExpression = val;
    },
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
        this.list = response.data;
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
        taskGroup: "",
        taskName: "",
        description: "",
        apiUrl: "",
        requestType: "Post",
        requestHead: "",
        requestParams: "",
        exceptionMsg: "",
        cronExpression: "",
        cronExpressionDescription: "",
        startRunTime: "",
        endRunTime: "",
        runCount: 0,
        taskState: 0
      };
    },
    //新增
    handleCreate() {
      this.resetTemp();
      this.dialogStatus = "create";
      this.dialogTitle = "新增任务配置";
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
          addOrEdit(data).then(response => {
            var d = response.data;
            this.list.unshift(d);
            this.total++;
            this.dialogFormVisible = false;
            myAction.getNotifyFunc(response, this);
          });
        }
      });
    },
    //启动
    handleStart(row) {
      let gId = row.id;
      addJob(gId).then(response => {
        myAction.getNotifyFunc(response, this);
      });
    },
    //暂停
    handleStop(row) {
      let gId = row.id;
      stopJob(gId).then(response => {
        myAction.getNotifyFunc(response, this);
      });
    },
    //恢复
    handleResume(row) {
      let gId = row.id;
      resumeJob(gId).then(response => {
        myAction.getNotifyFunc(response, this);
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
          let data={"":row.id};
          deleteJob(qs.stringify(data)).then(response => {
            if (response.resultSign == 0) {
              this.list.splice(index, 1);
              this.total--;
            }
            myAction.getNotifyFunc(response, this);
          });
        })
        .catch(action => {});
    }
  },
  mounted() {}
};
</script>

